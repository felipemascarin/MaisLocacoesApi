using FluentValidation;
using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
using MaisLocacoes.WebApi._2_Service.v1.Services;
using MaisLocacoes.WebApi._2_Service.v1.Services.Authentication;
using MaisLocacoes.WebApi._3_Repository.v1.Repository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Validator;
using MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema.Authentication;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.Repository.UserSchema;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Service.v1.Services.UserSchema;
using Repository.v1.IRepository;
using Repository.v1.IRepository.UserSchema;
using Repository.v1.Repository;
using Repository.v1.Repository.UserSchema;
using Service.v1.IServices;
using Service.v1.IServices.UserSchema;
using Service.v1.Services;
using Service.v1.Services.UserSchema;

namespace MaisLocacoes.WebApi.IoC
{
    public class Bootstrapper
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyAddressService, CompanyAddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICompanyTuitionService, CompanyTuitionService>();
            services.AddScoped<ICompanyWasteService, CompanyWasteService>();
            services.AddScoped<IOsService, OsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductTuitionService, ProductTuitionService>();
            services.AddScoped<IProductTuitionValueService, ProductTuitionValueService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IProductWasteService, ProductWasteService>();
            services.AddScoped<IQgService, QgService>();
            services.AddScoped<IRentedPlaceService, RentedPlaceService>();
            services.AddScoped<IRentService, RentService>();
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyAddressRepository, CompanyAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICompanyTuitionRepository, CompanyTuitionRepository>();
            services.AddScoped<ICompanyWasteRepository, CompanyWasteRepository>();
            services.AddScoped<IOsRepository, OsRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTuitionRepository, ProductTuitionRepository>();
            services.AddScoped<IProductTuitionValueRepository, ProductTuitionValueRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductWasteRepository, ProductWasteRepository>();
            services.AddScoped<IQgRepository, QgRepository>();
            services.AddScoped<IRentedPlaceRepository, RentedPlaceRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
            services.AddScoped<IValidator<TokenRequest>, TokenRequestValidator>();
            services.AddScoped<IValidator<CompanyRequest>, CompanyValidator>();
            services.AddScoped<IValidator<CompanyAddressRequest>, CompanyAddressValidator>();
            services.AddScoped<IValidator<UserRequest>, UserValidator>();
            services.AddScoped<IValidator<AddressRequest>, AddressValidator>();
            services.AddScoped<IValidator<BillRequest>, BillValidator>();
            services.AddScoped<IValidator<ClientRequest>, ClientValidator>();
            services.AddScoped<IValidator<CompanyTuitionRequest>, CompanyTuitionValidator>();
            services.AddScoped<IValidator<CompanyWasteRequest>, CompanyWasteValidator>();
            services.AddScoped<IValidator<OsRequest>, OsValidator>();
            services.AddScoped<IValidator<ProductTuitionRequest>, ProductTuitionValidator>();
            services.AddScoped<IValidator<ProductTuitionValueRequest>, ProductTuitionValueValidator>();
            services.AddScoped<IValidator<ProductTypeRequest>, ProductTypeValidator>();
            services.AddScoped<IValidator<ProductRequest>, ProductValidator>();
            services.AddScoped<IValidator<ProductWasteRequest>, ProductWasteValidator>();
            services.AddScoped<IValidator<QgRequest>, QgValidator>();
            services.AddScoped<IValidator<RentedPlaceRequest>, RentedPlaceValidator>();
            services.AddScoped<IValidator<RentRequest>, RentValidator>();
            services.AddScoped<IValidator<SupplierRequest>, SupplierValidator>();
        }
    }
}