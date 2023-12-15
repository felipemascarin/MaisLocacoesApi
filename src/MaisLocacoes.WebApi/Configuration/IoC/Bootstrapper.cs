using FluentValidation;
using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
using MaisLocacoes.WebApi._2_Service.v1.Services;
using MaisLocacoes.WebApi._2_Service.v1.Services.Authentication;
using MaisLocacoes.WebApi._2Service.v1.IServices;
using MaisLocacoes.WebApi._2Service.v1.Services;
using MaisLocacoes.WebApi._3_Repository.v1.Repository;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi._3Repository.v1.Repository;
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
            services.AddScoped<IContractService, ContractService>();
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
            services.AddScoped<ICompanyTuitionRepository, CompanyTuitionRepositoryRepository>();
            services.AddScoped<ICompanyWasteRepository, CompanyWasteRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
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

            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<LogoutRequest>, LogoutRequestValidator>();
            services.AddScoped<IValidator<CreateCompanyRequest>, CreateCompanyValidator>();
            services.AddScoped<IValidator<CreateCompanyAddressRequest>, CreateCompanyAddressValidator>();
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<CreateAddressRequest>, CreateAddressValidator>();
            services.AddScoped<IValidator<CreateBillRequest>, CreateBillValidator>();
            services.AddScoped<IValidator<CreateClientRequest>, CreateClientValidator>();
            services.AddScoped<IValidator<CreateCompanyTuitionRequest>, CreateCompanyTuitionValidator>();
            services.AddScoped<IValidator<CreateCompanyWasteRequest>, CreateCompanyWasteValidator>();
            services.AddScoped<IValidator<CreateContractRequest>, CreateContractValidator>();
            services.AddScoped<IValidator<CreateOsRequest>, CreateOsValidator>();
            services.AddScoped<IValidator<CreateProductTuitionRequest>, CreateProductTuitionValidator>();
            services.AddScoped<IValidator<CreateProductTuitionValueRequest>, CreateProductTuitionValueValidator>();
            services.AddScoped<IValidator<CreateProductTypeRequest>, CreateProductTypeValidator>();
            services.AddScoped<IValidator<CreateProductRequest>, CreateProductValidator>();
            services.AddScoped<IValidator<CreateProductWasteRequest>, CreateProductWasteValidator>();
            services.AddScoped<IValidator<CreateQgRequest>, CreateQgValidator>();
            services.AddScoped<IValidator<CreateRentedPlaceRequest>, CreateRentedPlaceValidator>();
            services.AddScoped<IValidator<CreateRentRequest>, CreateRentValidator>();
            services.AddScoped<IValidator<CreateSupplierRequest>, CreateSupplierValidator>();

            services.AddScoped<IValidator<UpdateCompanyRequest>, UpdateCompanyValidator>();
            services.AddScoped<IValidator<UpdateCompanyAddressRequest>, UpdateCompanyAddressValidator>();
            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();
            services.AddScoped<IValidator<UpdateAddressRequest>, UpdateAddressValidator>();
            services.AddScoped<IValidator<UpdateBillRequest>, UpdateBillValidator>();
            services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientValidator>();
            services.AddScoped<IValidator<UpdateCompanyTuitionRequest>, UpdateCompanyTuitionValidator>();
            services.AddScoped<IValidator<UpdateCompanyWasteRequest>, UpdateCompanyWasteValidator>();
            services.AddScoped<IValidator<UpdateContractRequest>, UpdateContractValidator>();
            services.AddScoped<IValidator<UpdateOsRequest>, UpdateOsValidator>();
            services.AddScoped<IValidator<UpdateProductTuitionRequest>, UpdateProductTuitionValidator>();
            services.AddScoped<IValidator<UpdateProductTuitionValueRequest>, UpdateProductTuitionValueValidator>();
            services.AddScoped<IValidator<UpdateProductTypeRequest>, UpdateProductTypeValidator>();
            services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductValidator>();
            services.AddScoped<IValidator<UpdateProductWasteRequest>, UpdateProductWasteValidator>();
            services.AddScoped<IValidator<UpdateQgRequest>, UpdateQgValidator>();
            services.AddScoped<IValidator<UpdateRentedPlaceRequest>, UpdateRentedPlaceValidator>();
            services.AddScoped<IValidator<UpdateRentRequest>, UpdateRentValidator>();
            services.AddScoped<IValidator<UpdateSupplierRequest>, UpdateSupplierValidator>();
        }
    }
}