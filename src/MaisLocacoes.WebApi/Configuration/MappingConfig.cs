using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetAllOsByStatusResponse;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetAllProductTuitionByProductIdResponse;

namespace Configuration
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateCompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CreateCompanyAddressResponse>();

                config.CreateMap<CreateAddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, CreateAddressResponse>();
                config.CreateMap<AddressEntity, GetAddressByIdResponse>();

                config.CreateMap<CreateCompanyRequest, CompanyEntity>();
                config.CreateMap<CompanyEntity, GetCompanyByCnpjResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));
                config.CreateMap<CompanyEntity, CreateCompanyResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));

                config.CreateMap<CreateUserRequest, UserEntity>();
                config.CreateMap<UserEntity, GetUserByEmailResponse>();
                config.CreateMap<UserEntity, GetUserByCpfResponse>();
                config.CreateMap<UserEntity, GetAllUsersByCnpjResponse>();
                config.CreateMap<UserEntity, CreateUserResponse>();

                config.CreateMap<CreateClientRequest, ClientEntity>();
                config.CreateMap<ClientEntity, GetClientByIdResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));
                config.CreateMap<ClientEntity, GetClientByIdDetailsResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));
                config.CreateMap<ClientEntity, GetClientByCpfResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));
                config.CreateMap<ClientEntity, GetClientByCnpjResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));
                config.CreateMap<ClientEntity, GetClientsByPageResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));
                config.CreateMap<ClientEntity, CreateClientResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));

                config.CreateMap<CreateBillRequest, BillEntity>();
                config.CreateMap<BillEntity, CreateBillResponse>();
                config.CreateMap<BillEntity, GetBillByRentIdResponse>();
                config.CreateMap<BillEntity, GetBillForTaxInvoiceResponse>();
                config.CreateMap<BillEntity, GetBillByIdResponse>();

                config.CreateMap<CreateCompanyTuitionRequest, CompanyTuitionEntity>();
                config.CreateMap<CompanyTuitionEntity, GetCompanyTuitionByIdResponse>();
                config.CreateMap<CompanyTuitionEntity, CreateCompanyTuitionResponse>();

                config.CreateMap<CreateCompanyWasteRequest, CompanyWasteEntity>();
                config.CreateMap<CompanyWasteEntity, GetCompanyWasteByIdResponse>();
                config.CreateMap<CompanyWasteEntity, CreateCompanyWasteResponse>();

                config.CreateMap<CreateOsRequest, OsEntity>();
                config.CreateMap<OsEntity, GetOsByIdResponse>();
                config.CreateMap<OsEntity, CreateOsResponse>();
                config.CreateMap<OsEntity, GetAllOsByStatusResponse>();
                config.CreateMap<OsEntity, GetOsByStatusRelationTuition>();
                config.CreateMap<GetOsByStatusRelationTuition, GetAllOsByStatusResponse>();

                config.CreateMap<CreateProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, GetProductByIdResponse>();
                config.CreateMap<ProductEntity, GetProductByTypeCodeResponse>();
                config.CreateMap<ProductEntity, GetProductsByPageResponse>();
                config.CreateMap<ProductEntity, CreateProductResponse>();

                config.CreateMap<CreateProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, CreateProductTuitionResponse>();
                config.CreateMap<ProductTuitionEntity, ResumedRentDto>();
                config.CreateMap<ProductTuitionEntity, GetAllProductTuitionByRentIdReponse>();
                config.CreateMap<ProductTuitionEntity, GetProductTuitionByIdResponse>()
               .ForMember(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity))
               .ForPath(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent.Address, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity.AddressEntity));

                config.CreateMap<CreateProductTuitionValueRequest, ProductTuitionValueEntity>();
                config.CreateMap<ProductTuitionValueEntity, GetAllProductTuitionToRemoveReponse>();
                config.CreateMap<ProductTuitionValueEntity, CreateProductTuitionValueResponse>();
                config.CreateMap<ProductTuitionValueEntity, GetProductTuitionValueByIdResponse>();
                config.CreateMap<ProductTuitionValueEntity, GetAllProductTuitionValueByProductTypeIdResponse>();

                config.CreateMap<CreateProductTypeRequest, ProductTypeEntity>();
                config.CreateMap<ProductTypeEntity, CreateProductTypeResponse>();
                config.CreateMap<ProductTypeEntity, GetProductTypeByIdResponse>();
                config.CreateMap<ProductTypeEntity, GetAllProductTypesResponse>();

                config.CreateMap<CreateProductWasteRequest, ProductWasteEntity>();
                config.CreateMap<ProductWasteEntity, CreateProductWasteResponse>();
                config.CreateMap<ProductWasteEntity, GetProductWasteByIdResponse>();
                config.CreateMap<ProductWasteEntity, GetProductWastesByPageResponse>();

                config.CreateMap<CreateQgRequest, QgEntity>();
                config.CreateMap<QgEntity, CreateQgResponse>();
                config.CreateMap<QgEntity, GetQgByIdResponse>();
                config.CreateMap<QgEntity, GetAllQgsResponse>();

                config.CreateMap<CreateRentedPlaceRequest, RentedPlaceEntity>();
                config.CreateMap<RentedPlaceEntity, GetRentedPlaceByIdResponse>();
                config.CreateMap<RentedPlaceEntity, CreateRentedPlaceResponse>();

                config.CreateMap<CreateRentRequest, RentEntity>();
                config.CreateMap<RentEntity, GetRentByIdResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity))
                .ForMember(RentResponse => RentResponse.Client, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity))
                .ForPath(RentResponse => RentResponse.Client.Address, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<RentEntity, GetAllRentsByClientIdResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity));
                config.CreateMap<RentEntity, CreateRentResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity));
                config.CreateMap<RentEntity, GetRentClientResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity))
                .ForMember(RentResponse => RentResponse.Client, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity))
                .ForPath(RentResponse => RentResponse.Client.Address, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<RentEntity, GetRentByPageResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity))
                .ForMember(RentResponse => RentResponse.Client, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity))
                .ForPath(RentResponse => RentResponse.Client.Address, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity.AddressEntity));

                config.CreateMap<CreateSupplierRequest, SupplierEntity>();
                config.CreateMap<SupplierEntity, GetSupplierByIdResponse>()
                .ForMember(SupplierResponse => SupplierResponse.Address, opt => opt.MapFrom(SupplierEntity => SupplierEntity.AddressEntity));
                config.CreateMap<SupplierEntity, GetAllSuppliersResponse>()
                .ForMember(SupplierResponse => SupplierResponse.Address, opt => opt.MapFrom(SupplierEntity => SupplierEntity.AddressEntity));
                config.CreateMap<SupplierEntity, CreateSupplierResponse>()
                .ForMember(SupplierResponse => SupplierResponse.Address, opt => opt.MapFrom(SupplierEntity => SupplierEntity.AddressEntity));
            });
            return mappingConfig;
        }
    }
}