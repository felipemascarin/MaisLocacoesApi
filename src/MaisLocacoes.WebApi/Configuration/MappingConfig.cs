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
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetOsByStatusResponse;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetProductTuitionProductIdResponse;

namespace Configuration
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CompanyAddressRequest, CompanyAddressEntity>();
                config.CreateMap<CompanyAddressEntity, CompanyAddressResponse>();

                config.CreateMap<AddressRequest, AddressEntity>();
                config.CreateMap<AddressEntity, AddressResponse>();

                config.CreateMap<CompanyRequest, CompanyEntity>();
                config.CreateMap<CompanyEntity, CompanyResponse>()
                .ForMember(CompanyResponse => CompanyResponse.CompanyAddress, opt => opt.MapFrom(CompanyEntity => CompanyEntity.CompanyAddressEntity));

                config.CreateMap<UserRequest, UserEntity>();
                config.CreateMap<UserEntity, UserResponse>();

                config.CreateMap<ClientRequest, ClientEntity>();
                config.CreateMap<ClientEntity, ClientResponse>()
                .ForMember(ClientResponse => ClientResponse.Address, opt => opt.MapFrom(ClientEntity => ClientEntity.AddressEntity));

                config.CreateMap<BillRequest, BillEntity>();
                config.CreateMap<BillEntity, BillResponse>();
                config.CreateMap<BillEntity, GetBillProductTypeForRentResponse>();
                config.CreateMap<BillEntity, GetBillForTaxInvoiceResponse>();

                config.CreateMap<CompanyTuitionRequest, CompanyTuitionEntity>();
                config.CreateMap<CompanyTuitionEntity, CompanyTuitionResponse>();

                config.CreateMap<CompanyWasteRequest, CompanyWasteEntity>();
                config.CreateMap<CompanyWasteEntity, CompanyWasteResponse>();

                config.CreateMap<OsRequest, OsEntity>();
                config.CreateMap<OsEntity, OsResponse>();
                config.CreateMap<OsEntity, GetOsByStatusResponse>();
                config.CreateMap<GetOsByStatusRelationTuition, GetOsByStatusResponse>();

                config.CreateMap<ProductRequest, ProductEntity>();
                config.CreateMap<ProductEntity, ProductResponse>();

                config.CreateMap<ProductTuitionRequest, ProductTuitionEntity>();
                config.CreateMap<ProductTuitionEntity, ProductTuitionResponse>();
                config.CreateMap<ProductTuitionEntity, ResumedRentDto>();
                config.CreateMap<ProductTuitionEntity, GetProductTuitionRentProductTypeClientReponse>();
                config.CreateMap<ProductTuitionEntity, GetProductTuitionRentResponse>()
               .ForMember(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity))
               .ForPath(GetProductTuitionRentResponse => GetProductTuitionRentResponse.Rent.Address, opt => opt.MapFrom(ProductTuitionEntity => ProductTuitionEntity.RentEntity.AddressEntity));

                config.CreateMap<ProductTuitionValueRequest, ProductTuitionValueEntity>();
                config.CreateMap<ProductTuitionValueEntity, ProductTuitionValueResponse>();

                config.CreateMap<ProductTypeRequest, ProductTypeEntity>();
                config.CreateMap<ProductTypeEntity, ProductTypeResponse>();

                config.CreateMap<ProductWasteRequest, ProductWasteEntity>();
                config.CreateMap<ProductWasteEntity, ProductWasteResponse>();

                config.CreateMap<QgRequest, QgEntity>();
                config.CreateMap<QgEntity, QgResponse>();

                config.CreateMap<RentedPlaceRequest, RentedPlaceEntity>();
                config.CreateMap<RentedPlaceEntity, RentedPlaceResponse>();

                config.CreateMap<RentRequest, RentEntity>();
                config.CreateMap<RentEntity, RentResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity));
                config.CreateMap<RentEntity, GetRentClientResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity))
                .ForMember(RentResponse => RentResponse.Client, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity))
                .ForPath(RentResponse => RentResponse.Client.Address, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity.AddressEntity));
                config.CreateMap<RentEntity, GetRentByPageResponse>()
                .ForMember(RentResponse => RentResponse.Address, opt => opt.MapFrom(RentEntity => RentEntity.AddressEntity))
                .ForMember(RentResponse => RentResponse.Client, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity))
                .ForPath(RentResponse => RentResponse.Client.Address, opt => opt.MapFrom(RentEntity => RentEntity.ClientEntity.AddressEntity));

                config.CreateMap<SupplierRequest, SupplierEntity>();
                config.CreateMap<SupplierEntity, SupplierResponse>()
                .ForMember(SupplierResponse => SupplierResponse.Address, opt => opt.MapFrom(SupplierEntity => SupplierEntity.AddressEntity));
            });
            return mappingConfig;
        }
    }
}