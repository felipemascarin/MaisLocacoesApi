using AutoMapper;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;
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
                config.CreateMap<CompanyRequest, CompanyEntity>();
                config.CreateMap<CompanyEntity, CreateCompanyResponse>();
                config.CreateMap<CompanyEntity, GetCompanyResponse>();

                config.CreateMap<CompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CreateCompanyAddressResponse>();
                config.CreateMap<CompanyAddressEntity, GetCompanyAddressResponse>();

                config.CreateMap<UserRequest, UserEntity>();
                config.CreateMap<UserEntity, CreateUserResponse>();
                config.CreateMap<UserEntity, GetUserResponse>();

                config.CreateMap<ClientRequest, ClientEntity>();
                config.CreateMap<ClientEntity, CreateClientResponse>();
                config.CreateMap<ClientEntity, GetClientResponse>();                

                config.CreateMap<AddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, CreateAddressResponse>();
                config.CreateMap<AddressEntity, GetAddressResponse>();

                config.CreateMap<BillRequest, BillEntity>();
                config.CreateMap<BillEntity, CreateBillResponse>();
                config.CreateMap<BillEntity, GetBillResponse>();

                config.CreateMap<CompanyTuitionRequest, CompanyTuitionEntity>();
                config.CreateMap<CompanyTuitionEntity, CreateCompanyTuitionResponse>();
                config.CreateMap<CompanyTuitionEntity, GetCompanyTuitionResponse>();

                config.CreateMap<CompanyWasteRequest, CompanyWasteEntity>();
                config.CreateMap<CompanyWasteEntity, CreateCompanyWasteResponse>();
                config.CreateMap<CompanyWasteEntity, GetCompanyWasteResponse>();

                config.CreateMap<OsRequest, OsEntity>();
                config.CreateMap<OsEntity, CreateOsResponse>();
                config.CreateMap<OsEntity, GetOsResponse>();

                config.CreateMap<ProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, CreateProductResponse>();
                config.CreateMap<ProductEntity, GetProductResponse>();

                config.CreateMap<ProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, CreateProductTuitionResponse>();
                config.CreateMap<ProductTuitionEntity, GetProductTuitionResponse>();

                config.CreateMap<ProductTypeRequest, ProductTypeEntity>();
                config.CreateMap<ProductTypeEntity, CreateProductTypeResponse>();
                config.CreateMap<ProductTypeEntity, GetProductTypeResponse>();

                config.CreateMap<ProductWasteRequest, ProductWasteEntity>();
                config.CreateMap<ProductWasteEntity, CreateProductWasteResponse>();
                config.CreateMap<ProductWasteEntity, GetProductWasteResponse>();

                config.CreateMap<QgRequest, QgEntity>();
                config.CreateMap<QgEntity, CreateQgResponse>();
                config.CreateMap<QgEntity, GetQgResponse>();

                config.CreateMap<RentedPlaceRequest, RentedPlaceEntity>();
                config.CreateMap<RentedPlaceEntity, CreateRentedPlaceResponse>();
                config.CreateMap<RentedPlaceEntity, GetRentedPlaceResponse>();

                config.CreateMap<RentRequest, RentEntity>();
                config.CreateMap<RentEntity, CreateRentResponse>();
                config.CreateMap<RentEntity, GetRentResponse>();

                config.CreateMap<SupplierRequest, SupplierEntity>();
                config.CreateMap<SupplierEntity, CreateSupplierResponse>();
                config.CreateMap<SupplierEntity, GetSupplierResponse>();

                config.CreateMap<UserEntity, UsersDeletions>();
                config.CreateMap<BillEntity, BillsDeletions>();
                config.CreateMap<ClientEntity, ClientsDeletions>();
                config.CreateMap<CompanyTuitionEntity, CompanyTuitionsDeletions>();
                config.CreateMap<CompanyWasteEntity, CompanyWastesDeletions>();
                config.CreateMap<ProductEntity, ProductsDeletions>();
                config.CreateMap<ProductTuitionEntity, ProductTuitionsDeletions>();
                config.CreateMap<ProductTypeEntity, ProductTypesDeletions>();
                config.CreateMap<ProductWasteEntity, ProductWastesDeletions>();
                config.CreateMap<QgEntity, QgsDeletions>();
                config.CreateMap<RentedPlaceEntity, RentedPlacesDeletions>();
                config.CreateMap<SupplierEntity, SuppliersDeletions>();
            });
            return mappingConfig;
        }
    }
}