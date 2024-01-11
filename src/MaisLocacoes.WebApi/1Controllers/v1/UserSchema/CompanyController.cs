using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices.UserSchema;

namespace MaisLocacoes.WebApi.Controllers.v1.UserSchema
{
    [Route("api/v1/company")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IValidator<CreateCompanyRequest> _createCompanyValidator;
        private readonly IValidator<UpdateCompanyRequest> _updateCompanyValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _email;
        private readonly string _cnpj;

        public CompanyController(ICompanyService companyService,
            IValidator<CreateCompanyRequest> createCompanyValidator,
            IValidator<UpdateCompanyRequest> updateCompanyValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _createCompanyValidator = createCompanyValidator;
            _updateCompanyValidator = updateCompanyValidator;
            _logger = loggerFactory.CreateLogger<CompanyController>();
            _httpContextAccessor = httpContextAccessor;
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize(Roles = "adm")]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest companyRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompany {@dateTime} UTC {@companyRequest} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, JsonConvert.SerializeObject(companyRequest), _email, _cnpj);

                var validatedCompany = _createCompanyValidator.Validate(companyRequest);

                if (!validatedCompany.IsValid)
                {
                    var companyValidationErros = new List<string>();
                    validatedCompany.Errors.ForEach(error => companyValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyValidationErros);
                }

                var companyCreated = await _companyService.CreateCompany(companyRequest);

                return Ok(companyCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByCnpj {@dateTime} UTC cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, cnpj, _email, _cnpj);

                var company = await _companyService.GetCompanyByCnpj(cnpj);
                return Ok(company);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpGet("bytoken")]
        public async Task<IActionResult> GetByToken()
        {
            try
            {
                _logger.LogInformation("GetByToken {@dateTime} UTC User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, _email, _cnpj);

                var company = await _companyService.GetCompanyByCnpj(JwtManager.GetCnpjByToken(_httpContextAccessor));
                return Ok(company);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpPut("{cnpj}")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest companyRequest, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateCompany {@dateTime} UTC {@companyRequest} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, JsonConvert.SerializeObject(companyRequest), cnpj, _email, _cnpj);

                var validatedCompany = _updateCompanyValidator.Validate(companyRequest);

                if (!validatedCompany.IsValid)
                {
                    var companyValidationErros = new List<string>();
                    validatedCompany.Errors.ForEach(error => companyValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyValidationErros);
                }

                await _companyService.UpdateCompany(companyRequest, cnpj);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpPut("cnpj/{cnpj}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, status, cnpj, _email, _cnpj);

                if (!CompanyStatus.CompanyStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                await _companyService.UpdateStatus(status, cnpj);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}