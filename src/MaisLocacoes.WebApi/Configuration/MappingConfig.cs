using AutoMapper;
using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Address;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Bill;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Client;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.CompanyTuition;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.CompanyWaste;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Contract;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Os;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Product;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuitionValue;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductWaste;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Qg;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.RentedPlace;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Supplier;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Company;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.CompanyAddress;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.User;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition.GetAllProductTuitionByProductIdResponse;

namespace Configuration
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //CompanyAddress
                config.CreateMap<CreateCompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CreateCompanyAddressResponse>();
                config.CreateMap<CompanyAddressEntity, GetCompanyAddressByIdResponse>();

                //Company
                config.CreateMap<CreateCompanyRequest, CompanyEntity>();
                config.CreateMap<CompanyEntity, GetCompanyByCnpjResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));
                config.CreateMap<CompanyEntity, CreateCompanyResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));

                //Address
                config.CreateMap<CreateAddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, CreateAddressResponse>();
                config.CreateMap<AddressEntity, GetAddressByIdResponse>();

                //User
                config.CreateMap<CreateUserRequest, UserEntity>();
                config.CreateMap<UserEntity, GetUserByEmailResponse>();
                config.CreateMap<UserEntity, GetUserByCpfResponse>();
                config.CreateMap<UserEntity, GetAllUsersByCnpjResponse>();
                config.CreateMap<UserEntity, CreateUserResponse>();

                //Client
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

                //Bill
                config.CreateMap<CreateBillRequest, BillEntity>();
                config.CreateMap<BillEntity, CreateBillResponse>();
                config.CreateMap<BillEntity, GetBillByRentIdResponse>()
                .ForMember(BillResponse => BillResponse.ProductType, opt => opt.MapFrom(BillEntity => BillEntity.ProductTuitionEntity.ProductTypeEntity));
                config.CreateMap<BillEntity, GetBillByIdResponse>();
                config.CreateMap<ProductTypeEntity, GetBillForTaxInvoiceResponse.ProductTypeResponse>();
                config.CreateMap<BillEntity, GetBillForTaxInvoiceResponse>()
                .ForMember(BillResponse => BillResponse.Rent, opt => opt.MapFrom(BillEntity => BillEntity.RentEntity))
                .ForPath(BillResponse => BillResponse.Rent.Address, opt => opt.MapFrom(BillEntity => BillEntity.RentEntity.AddressEntity))
                .ForPath(BillResponse => BillResponse.Rent.Client, opt => opt.MapFrom(BillEntity => BillEntity.RentEntity.ClientEntity))
                .ForPath(BillResponse => BillResponse.Rent.Client.Address, opt => opt.MapFrom(BillEntity => BillEntity.RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<CompanyEntity, GetBillForTaxInvoiceResponse.CompanyResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));

                //CompanyTuition
                config.CreateMap<CreateCompanyTuitionRequest, CompanyTuitionEntity>();
                config.CreateMap<CompanyTuitionEntity, GetCompanyTuitionByIdResponse>();
                config.CreateMap<CompanyTuitionEntity, CreateCompanyTuitionResponse>();

                //CompanyWaste
                config.CreateMap<CreateCompanyWasteRequest, CompanyWasteEntity>();
                config.CreateMap<CompanyWasteEntity, GetCompanyWasteByIdResponse>();
                config.CreateMap<CompanyWasteEntity, CreateCompanyWasteResponse>();

                //Contract
                config.CreateMap<CreateContractRequest, ContractEntity>();
                config.CreateMap<ContractEntity, CreateContractResponse>();
                config.CreateMap<ContractEntity, GetContractInfoByRentIdResponse>()
                .ForMember(dest => dest.Rent, opt => opt.MapFrom(src => src.RentEntity))
                .ForPath(dest => dest.Rent.Address, opt => opt.MapFrom(src => src.RentEntity.AddressEntity))
                .ForPath(dest => dest.Rent.Client, opt => opt.MapFrom(src => src.RentEntity.ClientEntity))
                .ForPath(dest => dest.Rent.Client.Address, opt => opt.MapFrom(src => src.RentEntity.ClientEntity.AddressEntity))
                .ForMember(dest => dest.ProductTuitions, opt => opt.MapFrom(src => src.RentEntity.ProductTuitions))
                .ForMember(dest => dest.ProductTuitions.Select(pt => pt.Product), opt => opt.MapFrom(src => src.RentEntity.ProductTuitions.Select(pt => pt.ProductEntity)))
                .ForMember(dest => dest.ProductTuitions.Select(pt => pt.Product.ProductType), opt => opt.MapFrom(src => src.RentEntity.ProductTuitions.Select(pt => pt.ProductEntity.ProductTypeEntity)))
                .ForMember(dest => dest.ProductTuitions.Select(pt => pt.Bills), opt => opt.MapFrom(src => src.RentEntity.ProductTuitions.Select(pt => pt.Bills)));
                config.CreateMap<CompanyEntity, GetContractInfoByRentIdResponse.ContractCompany>();
                config.CreateMap<CompanyAddressEntity, GetContractInfoByRentIdResponse.CompanyAddress>();
                config.CreateMap<ProductTypeEntity, GetContractInfoByRentIdResponse.ContractProductType>();
                config.CreateMap<ContractEntity, GetContractByIdResponse>()
                .ForMember(ContractResponse => ContractResponse.Rent, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity))S
                .ForPath(ContractResponse => ContractResponse.Rent.Address, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.AddressEntity))
                .ForPath(ContractResponse => ContractResponse.Rent.Client, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.ClientEntity))
                .ForPath(ContractResponse => ContractResponse.Rent.Client.Address, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<ContractEntity, GetAllContractsResponse>()
                .ForMember(ContractResponse => ContractResponse.Rent, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity))
                .ForPath(ContractResponse => ContractResponse.Rent.Address, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.AddressEntity))
                .ForPath(ContractResponse => ContractResponse.Rent.Client, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.ClientEntity))
                .ForPath(ContractResponse => ContractResponse.Rent.Client.Address, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity.ClientEntity.AddressEntity));

                //Os
                config.CreateMap<CreateOsRequest, OsEntity>();
                config.CreateMap<OsEntity, GetOsByIdResponse>();
                config.CreateMap<OsEntity, CreateOsResponse>();
                config.CreateMap<OsEntity, GetAllOsByStatusResponse>()
                .ForMember(OsResponse => OsResponse.ProductTuition, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity))
                .ForPath(OsResponse => OsResponse.ProductTuition.ProductType, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity.ProductTypeEntity))
                .ForPath(OsResponse => OsResponse.ProductTuition.Rent, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity.RentEntity))
                .ForPath(OsResponse => OsResponse.ProductTuition.Rent.Address, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity.RentEntity.AddressEntity))
                .ForPath(OsResponse => OsResponse.ProductTuition.Rent.Client, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity.RentEntity.ClientEntity))
                .ForPath(OsResponse => OsResponse.ProductTuition.Rent.Client.Address, opt => opt.MapFrom(OsEntity => OsEntity.ProductTuitionEntity.RentEntity.ClientEntity.AddressEntity));


                //Product
                config.CreateMap<CreateProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, GetProductByIdResponse>()
                .ForMember(ProductResponse => ProductResponse.ProductType, opt => opt.MapFrom(ProductEntity => ProductEntity.ProductTypeEntity));
                config.CreateMap<ProductEntity, GetProductByTypeCodeResponse>()
                .ForMember(ProductResponse => ProductResponse.ProductType, opt => opt.MapFrom(ProductEntity => ProductEntity.ProductTypeEntity));
                config.CreateMap<ProductEntity, GetProductsByPageResponse>()
                .ForMember(ProductResponse => ProductResponse.ProductType, opt => opt.MapFrom(ProductEntity => ProductEntity.ProductTypeEntity));
                config.CreateMap<ProductEntity, CreateProductResponse>()
                .ForMember(ProductResponse => ProductResponse.ProductType, opt => opt.MapFrom(ProductEntity => ProductEntity.ProductTypeEntity));

                //ProductTuition
                config.CreateMap<CreateProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<UpdateProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, CreateProductTuitionResponse>();
                config.CreateMap<ProductTuitionEntity, ResumedRentDto>();
                config.CreateMap<ProductTuitionEntity, GetAllProductTuitionByRentIdReponse>();
                config.CreateMap<ProductTuitionEntity, GetProductTuitionByIdResponse>()
                .ForMember(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity))
                .ForPath(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent.Address, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity.AddressEntity));

                //ProductTuitionValue
                config.CreateMap<CreateProductTuitionValueRequest, ProductTuitionValueEntity>();
                config.CreateMap<ProductTuitionValueEntity, GetAllProductTuitionToRemoveReponse>();
                config.CreateMap<ProductTuitionValueEntity, CreateProductTuitionValueResponse>();
                config.CreateMap<ProductTuitionValueEntity, GetProductTuitionValueByIdResponse>();
                config.CreateMap<ProductTuitionValueEntity, GetAllProductTuitionValueByProductTypeIdResponse>();

                //ProductType
                config.CreateMap<CreateProductTypeRequest, ProductTypeEntity>();
                config.CreateMap<ProductTypeEntity, CreateProductTypeResponse>();
                config.CreateMap<ProductTypeEntity, GetProductTypeByIdResponse>();
                config.CreateMap<ProductTypeEntity, GetAllProductTypesResponse>();

                //ProductWaste
                config.CreateMap<CreateProductWasteRequest, ProductWasteEntity>();
                config.CreateMap<ProductWasteEntity, CreateProductWasteResponse>();
                config.CreateMap<ProductWasteEntity, GetProductWasteByIdResponse>();
                config.CreateMap<ProductWasteEntity, GetProductWastesByPageResponse>();

                //Qg
                config.CreateMap<CreateQgRequest, QgEntity>();
                config.CreateMap<QgEntity, CreateQgResponse>();
                config.CreateMap<QgEntity, GetQgByIdResponse>();
                config.CreateMap<QgEntity, GetAllQgsResponse>();

                //RentedPlace
                config.CreateMap<CreateRentedPlaceRequest, RentedPlaceEntity>();
                config.CreateMap<RentedPlaceEntity, GetRentedPlaceByIdResponse>();
                config.CreateMap<RentedPlaceEntity, CreateRentedPlaceResponse>();

                //Rent
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

                //Supplier
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