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
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Company;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.CompanyAddress;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.User;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Configuration
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //Authentication
                config.CreateMap<CompanyEntity, LoginResponse.CompanyUserDto>();

                //CompanyAddress
                config.CreateMap<CreateCompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CreateCompanyAddressResponse>();
                config.CreateMap<CompanyAddressEntity, GetCompanyAddressByIdResponse>();
                config.CreateMap<CreateCompanyAddressResponse, CompanyAddressEntity>();

                //Company
                config.CreateMap<CreateCompanyRequest, CompanyEntity>();

                config.CreateMap<CompanyEntity, GetAllCompanyResponse.CompanyResponse>();
                config.CreateMap<CompanyAddressEntity, GetAllCompanyResponse.CompanyAddressResponse>();

                config.CreateMap<CompanyEntity, GetCompanyByCnpjResponse>();
                config.CreateMap<CompanyAddressEntity, GetCompanyByCnpjResponse.CompanyAddressResponse>();

                config.CreateMap<CompanyEntity, CreateCompanyResponse>();
                config.CreateMap<CompanyAddressEntity, CreateCompanyResponse.CompanyAddressResponse>();

                //Address
                config.CreateMap<CreateAddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, CreateAddressResponse>();
                config.CreateMap<CreateAddressResponse, AddressEntity>();
                config.CreateMap<AddressEntity, GetAddressByIdResponse>();

                //User
                config.CreateMap<CreateUserRequest, UserEntity>();
                config.CreateMap<UserEntity, GetUserByEmailResponse>();
                config.CreateMap<UserEntity, GetUserByCpfResponse>();

                config.CreateMap<UserEntity, GetAllUsersByCnpjResponse>();

                config.CreateMap<UserEntity, CreateUserResponse>();

                //Client
                config.CreateMap<CreateClientRequest, ClientEntity>();

                config.CreateMap<ClientEntity, GetClientByIdResponse>();
                config.CreateMap<AddressEntity, GetClientByIdResponse.AddressResponse>();

                config.CreateMap<ClientEntity, GetClientByIdDetailsResponse>();
                config.CreateMap<AddressEntity, GetClientByIdDetailsResponse.AddressResponse>();

                config.CreateMap<ClientEntity, GetClientByCpfResponse>();
                config.CreateMap<AddressEntity, GetClientByCpfResponse.AddressResponse>();

                config.CreateMap<ClientEntity, GetClientByCnpjResponse>();
                config.CreateMap<AddressEntity, GetClientByCnpjResponse.AddressResponse>();

                config.CreateMap<ClientEntity, GetClientsByPageResponse>();
                config.CreateMap<AddressEntity, GetClientsByPageResponse.AddressResponse>();

                config.CreateMap<ClientEntity, CreateClientResponse>();
                config.CreateMap<AddressEntity, CreateClientResponse.AddressResponse>();

                //Bill
                config.CreateMap<CreateBillRequest, BillEntity>();
                config.CreateMap<BillEntity, CreateBillResponse>();
                config.CreateMap<BillEntity, GetBillByRentIdResponse>();
                config.CreateMap<BillEntity, GetBillByIdResponse>();

                config.CreateMap<BillEntity, GetBillForTaxInvoiceResponse>();
                config.CreateMap<RentEntity, GetBillForTaxInvoiceResponse.RentResponse>();
                config.CreateMap<ClientEntity, GetBillForTaxInvoiceResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetBillForTaxInvoiceResponse.AddressResponse>();
                config.CreateMap<ProductTypeEntity, GetBillForTaxInvoiceResponse.ProductTypeResponse>();
                config.CreateMap<CompanyEntity, GetBillForTaxInvoiceResponse.CompanyResponse>();
                config.CreateMap<CompanyAddressEntity, GetBillForTaxInvoiceResponse.CompanyAddressResponse>();


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

                config.CreateMap<ContractEntity, GetContractInfoByRentIdResponse>();
                config.CreateMap<RentEntity, GetContractInfoByRentIdResponse.ContractRent>();
                config.CreateMap<ProductTuitionEntity, GetContractInfoByRentIdResponse.ContractProductTuition>();
                config.CreateMap<ProductEntity, GetContractInfoByRentIdResponse.ContractProduct>();
                config.CreateMap<ProductTypeEntity, GetContractInfoByRentIdResponse.ContractProductType>();
                config.CreateMap<BillEntity, GetContractInfoByRentIdResponse.ContractBill>();
                config.CreateMap<ClientEntity, GetContractInfoByRentIdResponse.ContractClient>();
                config.CreateMap<AddressEntity, GetContractInfoByRentIdResponse.ContractAddress>();
                config.CreateMap<CompanyEntity, GetContractInfoByRentIdResponse.ContractCompany>();
                config.CreateMap<CompanyAddressEntity, GetContractInfoByRentIdResponse.CompanyAddress>();

                config.CreateMap<ContractEntity, GetContractByIdResponse>();
                config.CreateMap<RentEntity, GetContractByIdResponse.RentResponse>();
                config.CreateMap<ClientEntity, GetContractByIdResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetContractByIdResponse.AddressResponse>();

                config.CreateMap<ContractEntity, GetAllContractsResponse>();
                config.CreateMap<RentEntity, GetAllContractsResponse.RentResponse>();
                config.CreateMap<ClientEntity, GetAllContractsResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetAllContractsResponse.AddressResponse>();

                //Os
                config.CreateMap<CreateOsRequest, OsEntity>();
                config.CreateMap<OsEntity, GetOsByIdResponse>();
                config.CreateMap<OsEntity, CreateOsResponse>();

                config.CreateMap<OsEntity, GetAllOsByStatusResponse>();
                config.CreateMap<ProductTuitionEntity, GetAllOsByStatusResponse.ProductTuitionResponse>();
                config.CreateMap<RentEntity, GetAllOsByStatusResponse.RentResponse>();
                config.CreateMap<ClientEntity, GetAllOsByStatusResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetAllOsByStatusResponse.AddressResponse>();
                config.CreateMap<ProductTypeEntity, GetAllOsByStatusResponse.ProductTypeResponse>();


                //Product
                config.CreateMap<CreateProductRequest, ProductEntity>();

                config.CreateMap<ProductEntity, GetProductByIdResponse>();
                config.CreateMap<ProductTypeEntity, GetProductByIdResponse.ProductTypeResponse>();

                config.CreateMap<ProductEntity, GetProductByTypeCodeResponse>();
                config.CreateMap<ProductTypeEntity, GetProductByTypeCodeResponse.ProductTypeResponse>();

                config.CreateMap<ProductEntity, GetProductsByPageResponse>();
                config.CreateMap<ProductTypeEntity, GetProductsByPageResponse.ProductTypeResponse>();

                config.CreateMap<ProductEntity, CreateProductResponse>();
                config.CreateMap<ProductTypeEntity, CreateProductResponse.ProductTypeResponse>();

                //ProductTuition
                config.CreateMap<CreateProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<UpdateProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, CreateProductTuitionResponse>();

                config.CreateMap<ProductTuitionEntity, GetAllProductTuitionByProductIdResponse.ResumedProductRentDto>();

                config.CreateMap<ProductTuitionEntity, GetAllProductTuitionByRentIdReponse>();
                config.CreateMap<RentEntity, GetAllProductTuitionByRentIdReponse.RentResponse>();
                config.CreateMap<ClientEntity, GetAllProductTuitionByRentIdReponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetAllProductTuitionByRentIdReponse.AddressResponse>();
                config.CreateMap<ProductTypeEntity, GetAllProductTuitionByRentIdReponse.ProductTypeResponse>();

                config.CreateMap<ProductTuitionEntity, GetProductTuitionByIdResponse>();
                config.CreateMap<RentEntity, GetProductTuitionByIdResponse.RentResponse>();
                config.CreateMap<AddressEntity, GetProductTuitionByIdResponse.AddressResponse>();

                //ProductTuitionValue
                config.CreateMap<CreateProductTuitionValueRequest, ProductTuitionValueEntity>();

                config.CreateMap<ProductTuitionEntity, GetAllProductTuitionToRemoveReponse>()
                .ForMember(dest => dest.Rent, opt => opt.MapFrom(src => src.Rent))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType));
                config.CreateMap<RentEntity, GetAllProductTuitionToRemoveReponse.RentResponse>();
                config.CreateMap<ClientEntity, GetAllProductTuitionToRemoveReponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetAllProductTuitionToRemoveReponse.AddressResponse>();
                config.CreateMap<ProductTypeEntity, GetAllProductTuitionToRemoveReponse.ProductTypeResponse>();


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
                config.CreateMap<AddressEntity, CreateQgResponse.AddressResponse>();
                config.CreateMap<RentedPlaceEntity, CreateQgResponse.RentedPlaceResponse>();

                config.CreateMap<QgEntity, GetQgByIdResponse>();
                config.CreateMap<AddressEntity, GetQgByIdResponse.AddressResponse>();
                config.CreateMap<RentedPlaceEntity, GetQgByIdResponse.RentedPlaceResponse>();

                config.CreateMap<QgEntity, GetAllQgsResponse>();
                config.CreateMap<AddressEntity, GetAllQgsResponse.AddressResponse>();
                config.CreateMap<RentedPlaceEntity, GetAllQgsResponse.RentedPlaceResponse>();

                //RentedPlace
                config.CreateMap<CreateRentedPlaceRequest, RentedPlaceEntity>();

                config.CreateMap<RentedPlaceEntity, GetRentedPlaceByIdResponse>();

                config.CreateMap<RentedPlaceEntity, CreateRentedPlaceResponse>();

                //Rent
                config.CreateMap<CreateRentRequest, RentEntity>();
                config.CreateMap<RentEntity, GetRentByIdResponse>();
                config.CreateMap<ClientEntity, GetRentByIdResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetRentByIdResponse.AddressResponse>();

                config.CreateMap<RentEntity, GetAllRentsByClientIdResponse.ResumedClientRentDto>();
                config.CreateMap<AddressEntity, GetAllRentsByClientIdResponse.AddressResponse>();

                config.CreateMap<RentEntity, CreateRentResponse>();
                config.CreateMap<AddressEntity, CreateRentResponse.AddressResponse>();

                config.CreateMap<RentEntity, GetRentClientResponse>();
                config.CreateMap<ClientEntity, GetRentClientResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetRentClientResponse.AddressResponse>();

                config.CreateMap<RentEntity, GetRentByPageResponse>();
                config.CreateMap<ClientEntity, GetRentByPageResponse.ClientResponse>();
                config.CreateMap<AddressEntity, GetRentByPageResponse.AddressResponse>();

                //Supplier
                config.CreateMap<CreateSupplierRequest, SupplierEntity>();
                config.CreateMap<SupplierEntity, GetSupplierByIdResponse>();
                config.CreateMap<AddressEntity, GetSupplierByIdResponse.AddressResponse>();

                config.CreateMap<SupplierEntity, GetAllSuppliersResponse>();
                config.CreateMap<AddressEntity, GetAllSuppliersResponse.AddressResponse>();

                config.CreateMap<SupplierEntity, CreateSupplierResponse>();
                config.CreateMap<AddressEntity, CreateSupplierResponse.AddressResponse>();
            });
            return mappingConfig;
        }
    }
}