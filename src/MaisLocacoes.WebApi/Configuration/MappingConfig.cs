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
                config.CreateMap<BillEntity, GetBillByRentIdResponse>();
                config.CreateMap<BillEntity, GetBillForTaxInvoiceResponse>();
                config.CreateMap<BillEntity, GetBillByIdResponse>();

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
                .ForPath(dest => dest.Rent.Client.Address, opt => opt.MapFrom(src => src.RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<RentEntity, GetContractInfoByRentIdResponse.ContractRent>();
                config.CreateMap<AddressEntity, GetContractInfoByRentIdResponse.ContractAddress>();
                config.CreateMap<ClientEntity, GetContractInfoByRentIdResponse.ContractClient>();
                config.CreateMap<ProductTuitionEntity, GetContractInfoByRentIdResponse.ContractProductTuition>();
                config.CreateMap<ProductEntity, GetContractInfoByRentIdResponse.ContractProduct>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductTypeEntity));
                config.CreateMap<ProductTypeEntity, GetContractInfoByRentIdResponse.ContractProductType>();
                config.CreateMap<ContractEntity, GetContractByIdResponse>()
                .ForMember(ContractResponse => ContractResponse.Rent, opt => opt.MapFrom(ContractEntity => ContractEntity.RentEntity))
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
                config.CreateMap<OsEntity, GetAllOsByStatusResponse>();
                config.CreateMap<OsEntity, GetOsByStatusRelationTuition>();
                config.CreateMap<GetOsByStatusRelationTuition, GetAllOsByStatusResponse>();

                //Product
                config.CreateMap<CreateProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, GetProductByIdResponse>();
                config.CreateMap<ProductEntity, GetProductByTypeCodeResponse>();
                config.CreateMap<ProductEntity, GetProductsByPageResponse>();
                config.CreateMap<ProductEntity, CreateProductResponse>();

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