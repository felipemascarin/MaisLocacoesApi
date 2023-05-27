using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
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
                config.CreateMap<CompanyEntity, CompanyResponse>();

                config.CreateMap<CompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CompanyAddressResponse>();

                config.CreateMap<UserRequest, UserEntity>();
                config.CreateMap<UserEntity, UserResponse>();

                config.CreateMap<ClientRequest, ClientEntity>();
                config.CreateMap<ClientEntity, ClientResponse>();            

                config.CreateMap<AddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, AddressResponse>();

                config.CreateMap<BillRequest, BillEntity>();
                config.CreateMap<BillEntity, BillResponse>();

                config.CreateMap<CompanyTuitionRequest, CompanyTuitionEntity>();
                config.CreateMap<CompanyTuitionEntity, CompanyTuitionResponse>();

                config.CreateMap<CompanyWasteRequest, CompanyWasteEntity>();
                config.CreateMap<CompanyWasteEntity, CompanyWasteResponse>();

                config.CreateMap<OsRequest, OsEntity>();
                config.CreateMap<OsEntity, OsResponse>();

                config.CreateMap<ProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, ProductResponse>();

                config.CreateMap<ProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, ProductTuitionResponse>();

                config.CreateMap<ProductTypeRequest, ProductTypeEntity>();
                config.CreateMap<ProductTypeEntity, ProductTypeResponse>();

                config.CreateMap<ProductWasteRequest, ProductWasteEntity>();
                config.CreateMap<ProductWasteEntity, ProductWasteResponse>();

                config.CreateMap<QgRequest, QgEntity>();
                config.CreateMap<QgEntity, QgResponse>();

                config.CreateMap<RentedPlaceRequest, RentedPlaceEntity>();
                config.CreateMap<RentedPlaceEntity, RentedPlaceResponse>();

                config.CreateMap<RentRequest, RentEntity>();
                config.CreateMap<RentEntity, RentResponse>();

                config.CreateMap<SupplierRequest, SupplierEntity>();
                config.CreateMap<SupplierEntity, SupplierResponse>();
            });
            return mappingConfig;
        }
    }
}