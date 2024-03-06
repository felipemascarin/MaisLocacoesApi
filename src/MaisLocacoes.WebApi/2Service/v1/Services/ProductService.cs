using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Product;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Product.GetAllProductPlacesDTO;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Product.GetAllProductPlacesResponse;

namespace Service.v1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ProductService(IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IProductTuitionRepository productTuitionRepository,
            IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _productTypeRepository = productTypeRepository;
            _rentedPlaceRepository = rentedPlaceRepository;
            _productTuitionRepository = productTuitionRepository;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest productRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productRequest = TimeZoneConverter<CreateProductRequest>.ConvertToTimeZoneLocal(productRequest, _timeZone);

            var userModule = JwtManager.GetModuleByToken(_httpContextAccessor);

            if (userModule != ProjectModules.Modules.ElementAt(0) /*basic*/)
            {
                if (productRequest.RentedPlaceId == null)
                    throw new HttpRequestException("Id da localização do QG é obrigatório", null, HttpStatusCode.BadRequest);
            }

            var productroductTypeEntity = await _productTypeRepository.GetById(productRequest.ProductTypeId.Value) ??
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            if (!productroductTypeEntity.IsManyParts)
            {
                if (productRequest.Parts > 1)
                    throw new HttpRequestException("Só é possível cadastrar 1 unidade desse tipo de produto, não possui partes", null, HttpStatusCode.BadRequest);
            }

            var product = await _productRepository.GetByTypeCode(productRequest.ProductTypeId.Value, productRequest.Code);
            if (product != null)
                throw new HttpRequestException("Produto já cadastrado", null, HttpStatusCode.BadRequest);

            if (productRequest.RentedParts > productRequest.Parts)
                throw new HttpRequestException("Não é possível alugar mais peças do que existe no produto", null, HttpStatusCode.BadRequest);

            var dateTimeNow = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            var productEntity = _mapper.Map<ProductEntity>(productRequest);

            productEntity.ProductType = productroductTypeEntity;
            productEntity.CreatedBy = _email;
            productEntity.CreatedAt = dateTimeNow;

            if (userModule != ProjectModules.Modules.ElementAt(0) /*basic*/)
            {
                var rentedPlaceQgEntity = await _rentedPlaceRepository.GetById(productRequest.RentedPlaceId.Value) ??
                throw new HttpRequestException("Antes de criar produto deve existir uma localização pelo menos - RentedId", null, HttpStatusCode.BadRequest);

                productEntity = await _productRepository.CreateProduct(productEntity);

                var rentedPlaceProductEntity = new RentedPlaceEntity()
                {
                    ProductId = productEntity.Id,
                    Latitude = rentedPlaceQgEntity.Latitude,
                    Longitude = rentedPlaceQgEntity.Longitude,
                    ArrivalDate = dateTimeNow,
                    CreatedBy = _email,
                    CreatedAt = dateTimeNow
                };

                await _rentedPlaceRepository.CreateRentedPlace(rentedPlaceProductEntity);
            }
            else
            {
                productEntity = await _productRepository.CreateProduct(productEntity);
            }

            var productResponse = _mapper.Map<CreateProductResponse>(productEntity);

            return productResponse;
        }

        public async Task<GetProductByIdResponse> GetProductById(int id)
        {
            var productEntity = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productResponse = _mapper.Map<GetProductByIdResponse>(productEntity);

            return productResponse;
        }

        public async Task<GetProductByTypeCodeResponse> GetProductByTypeCode(int typeId, string code)
        {
            var productEntity = await _productRepository.GetByTypeCode(typeId, code) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productResponse = _mapper.Map<GetProductByTypeCodeResponse>(productEntity);

            return productResponse;
        }

        public async Task<IEnumerable<GetProductsByPageResponse>> GetProductsByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var productsEntityList = await _productRepository.GetProductsByPage(items, page, query);

            var productsResponseList = _mapper.Map<IEnumerable<GetProductsByPageResponse>>(productsEntityList);

            return productsResponseList;
        }

        public async Task<IEnumerable<GetProductsForRentResponse>> GetProductsForRent(int productTypeId)
        {
            var productForRentDtoResponse = await _productRepository.GetProductsForRent(productTypeId);

            var productForRentResponse = new List<GetProductsForRentResponse>();

            foreach (var item in productForRentDtoResponse)
            {
                productForRentResponse.Add(new GetProductsForRentResponse
                {
                    Code = item.Code,
                    FreeParts = item.Parts - item.RentedParts
                });
            }

            return productForRentResponse.OrderBy(p => p.Code);
        }

        public async Task<GetAllProductPlacesResponse> GetAllProductPlaces()
        {
            var manyPartsProductsIdsList = new List<int>();
            var manyPartsProductsList = new List<ProductEntity>();
            var singlePartProductsList = new List<ProductEntity>();

            var productsList = await _productRepository.GetAllProducts();

            //Busca todos os produtos e separa em 2 listas (1 lista de produtos de muitas partes e 1 lista de produtos com 1 única parte)
            foreach (var product in productsList)
            {
                if (product.ProductType.IsManyParts)
                {
                    manyPartsProductsList.Add(product);
                    manyPartsProductsIdsList.Add(product.Id);
                }
                else
                    singlePartProductsList.Add(product);
            }

            //Buscar todos os produtos de muitas partes alugados
            var rentedManyPartsProductTuitionList = (await _productTuitionRepository.GetAllByProductIdForRentedPlaces(manyPartsProductsIdsList)).ToList();

            var productsIdsList = new List<int>();
            rentedManyPartsProductTuitionList.ForEach(r => productsIdsList.Add(r.Id));
            manyPartsProductsList.ForEach(m => productsIdsList.Add(m.Id));
            singlePartProductsList.ForEach(s => productsIdsList.Add(s.Id));

            //Buscar todos os últimos places de todos os produtos e producttuitions de many partes
            var rentedPlacesList = (await _rentedPlaceRepository.GetTheLastRentedPlaceByIds(productsIdsList)).ToList();

            var responseDTO = new GetAllProductPlacesDTO();

            foreach (var manyPartsProductTuition in rentedManyPartsProductTuitionList)
            {
                var productPlace = new GetAllProductPlacesDTO.ProductPlace();

                productPlace.IsManyParts = true;
                productPlace.Parts = manyPartsProductTuition.Parts;

                foreach (var rentedPlace in rentedPlacesList)
                {
                    if (rentedPlace.ProductTuitionId == manyPartsProductTuition.Id)
                    {
                        productPlace.Latitude = rentedPlace.Latitude;
                        productPlace.Longitude = rentedPlace.Longitude;
                    }
                }

                foreach (var manyPartsProduct in manyPartsProductsList)
                {
                    if (manyPartsProduct.Id == manyPartsProductTuition.ProductId)
                    {
                        productPlace.Type = manyPartsProduct.ProductType.Type;
                        productPlace.Code = manyPartsProduct.Code;
                        productPlace.Status = ProductStatus.ProductStatusEnum.ElementAt(1); /*rented*/
                    }
                }

                responseDTO.RentedProducts.Add(productPlace);
            }

            foreach (var manyPartsProduct in manyPartsProductsList)
            {
                var productPlace = new GetAllProductPlacesDTO.ProductPlace();

                productPlace.Parts = manyPartsProduct.Parts - manyPartsProduct.RentedParts;
                if (productPlace.Parts == 0) continue;

                productPlace.IsManyParts = true;
                productPlace.Type = manyPartsProduct.ProductType.Type;
                productPlace.Code = manyPartsProduct.Code;
                productPlace.Status = manyPartsProduct.Status;

                foreach (var rentedPlace in rentedPlacesList)
                {
                    if (rentedPlace.ProductId == manyPartsProduct.Id)
                    {
                        productPlace.Latitude = rentedPlace.Latitude;
                        productPlace.Longitude = rentedPlace.Longitude;
                    }
                }

                responseDTO.FreeProducts.Add(productPlace);
            }

            foreach (var singlePartProduct in singlePartProductsList)
            {
                var productPlace = new GetAllProductPlacesDTO.ProductPlace();

                productPlace.Type = singlePartProduct.ProductType.Type;
                productPlace.IsManyParts = false;
                productPlace.Code = singlePartProduct.Code;
                productPlace.Parts = 1;
                productPlace.Status = singlePartProduct.Status;

                foreach (var rentedPlace in rentedPlacesList)
                {
                    if (rentedPlace.ProductId == singlePartProduct.Id)
                    {
                        productPlace.Latitude = rentedPlace.Latitude;
                        productPlace.Longitude = rentedPlace.Longitude;
                    }
                }

                if (singlePartProduct.RentedParts == 1)
                    responseDTO.RentedProducts.Add(productPlace);
                else
                    responseDTO.FreeProducts.Add(productPlace);
            }

            var samePinRentedProducts = new List<GetAllProductPlacesResponse.PinResponse>();
            var samePinFreeProducts = new List<GetAllProductPlacesResponse.PinResponse>();

            var samePinProducts = new List<ProductsPinResponse>();
            var sameProductsNotRepeat = new List<ProductPlace>();

            foreach (var rentedProductFirstIteration in responseDTO.RentedProducts)
            {
                samePinProducts = new List<ProductsPinResponse>();

                foreach (var rentedProductSecondIteration in responseDTO.RentedProducts)
                {
                    //Se tiver a mesma coordenada e for produto diferente:
                    if (!sameProductsNotRepeat.Any(p => p.Latitude == rentedProductSecondIteration.Latitude && p.Longitude == rentedProductSecondIteration.Longitude))
                    {
                        if (rentedProductFirstIteration.Latitude == rentedProductSecondIteration.Latitude && rentedProductFirstIteration.Longitude == rentedProductSecondIteration.Longitude)
                        {
                            samePinProducts.Add(new ProductsPinResponse()
                            {
                                Type = rentedProductSecondIteration.Type,
                                IsManyParts = rentedProductSecondIteration.IsManyParts,
                                Code = rentedProductSecondIteration.Code,
                                Parts = rentedProductSecondIteration.Parts,
                                Status = ProductStatus.ProductStatusEnum.ElementAt(1) /*rented*/,
                            });

                        }
                    }
                }

                if (samePinProducts.Count > 0)
                {
                    samePinRentedProducts.Add(new GetAllProductPlacesResponse.PinResponse()
                    {
                        Latitude = rentedProductFirstIteration.Latitude,
                        Longitude = rentedProductFirstIteration.Longitude,
                        Products = samePinProducts
                    });
                }

                sameProductsNotRepeat.Add(rentedProductFirstIteration);
            }

            sameProductsNotRepeat = new List<ProductPlace>();

            foreach (var freeProductFirstIteration in responseDTO.FreeProducts)
            {
                samePinProducts = new List<ProductsPinResponse>();

                foreach (var freeProductSecondIteration in responseDTO.FreeProducts)
                {
                    //Se tiver a mesma coordenada e for produto diferente:
                    if (!sameProductsNotRepeat.Any(p => p.Latitude == freeProductSecondIteration.Latitude && p.Longitude == freeProductSecondIteration.Longitude))
                    {
                        if (freeProductFirstIteration.Latitude == freeProductSecondIteration.Latitude && freeProductFirstIteration.Longitude == freeProductSecondIteration.Longitude)
                        {
                            samePinProducts.Add(new ProductsPinResponse()
                            {
                                Type = freeProductSecondIteration.Type,
                                IsManyParts = freeProductSecondIteration.IsManyParts,
                                Code = freeProductSecondIteration.Code,
                                Parts = freeProductSecondIteration.Parts,
                                Status = freeProductSecondIteration.Status,
                            });

                        }
                    }
                }

                if (samePinProducts.Count > 0)
                {
                    samePinFreeProducts.Add(new GetAllProductPlacesResponse.PinResponse()
                    {
                        Latitude = freeProductFirstIteration.Latitude,
                        Longitude = freeProductFirstIteration.Longitude,
                        Products = samePinProducts
                    });
                }

                sameProductsNotRepeat.Add(freeProductFirstIteration);
            }

            return new GetAllProductPlacesResponse()
            {
                RentedProducts = samePinRentedProducts,
                FreeProducts = samePinFreeProducts
            };
        }

        public async Task UpdateProduct(UpdateProductRequest productRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productRequest = TimeZoneConverter<UpdateProductRequest>.ConvertToTimeZoneLocal(productRequest, _timeZone);

            var productForUpdate = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.BadRequest);

            if (productRequest.ProductTypeId != productForUpdate.ProductTypeId || productRequest.Code.ToLower() != productForUpdate.Code.ToLower())
            {
                if (productRequest.ProductTypeId != productForUpdate.ProductTypeId)
                {
                    var existsProductType = await _productTypeRepository.ProductTypeExists(productRequest.ProductTypeId.Value);
                    if (!existsProductType)
                        throw new HttpRequestException("Esse tipo de produto não existe", null, HttpStatusCode.BadRequest);
                }

                var existsTypeCode = await _productRepository.ProductExists(productRequest.ProductTypeId.Value, productRequest.Code);
                if (existsTypeCode)
                    throw new HttpRequestException("O código para esse tipo do produto já existe", null, HttpStatusCode.BadRequest);
            }

            if (productRequest.RentedParts > productRequest.Parts)
                throw new HttpRequestException("Não é possível alugar mais peças do que existe no produto", null, HttpStatusCode.BadRequest);

            productForUpdate.ProductTypeId = productRequest.ProductTypeId.Value;
            productForUpdate.Code = productRequest.Code;
            productForUpdate.SupplierId = productRequest.SupplierId;
            productForUpdate.Description = productRequest.Description;
            productForUpdate.DateBought = productRequest.DateBought;
            productForUpdate.BoughtValue = productRequest.BoughtValue;
            productForUpdate.Parts = productRequest.Parts.Value;
            productForUpdate.RentedParts = productRequest.RentedParts.Value;
            productForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForUpdate.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForUpdate);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var productForUpdate = await _productRepository.GetById(id) ??
                    throw new HttpRequestException("produto não encontrado", null, HttpStatusCode.NotFound);

            productForUpdate.Status = status;
            productForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForUpdate.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var productForDelete = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            productForDelete.Deleted = true;
            productForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForDelete.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForDelete);
        }
    }
}