using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
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
        private readonly TimeSpan _timeZone;
        private readonly string _email;
        private readonly string _schema;

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
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest companyRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompany {@dateTime} {@companyRequest} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(companyRequest), _email, _schema);

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
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByCnpj {@dateTime} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, cnpj, _email, _schema);

                var company = await _companyService.GetCompanyByCnpj(cnpj);
                return Ok(company);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("bytoken")]
        public async Task<IActionResult> GetByToken()
        {
            try
            {
                _logger.LogInformation("GetByToken {@dateTime} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, _email, _schema);

                var company = await _companyService.GetCompanyByCnpj(JwtManager.GetSchemaByToken(_httpContextAccessor));
                return Ok(company);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPut("{cnpj}")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest companyRequest, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateCompany {@dateTime} {@companyRequest} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(companyRequest), cnpj, _email, _schema);

                var validatedCompany = _updateCompanyValidator.Validate(companyRequest);

                if (!validatedCompany.IsValid)
                {
                    var companyValidationErros = new List<string>();
                    validatedCompany.Errors.ForEach(error => companyValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyValidationErros);
                }

                if (await _companyService.UpdateCompany(companyRequest, cnpj)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar a empresa"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPut("cnpj/{cnpj}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, status, cnpj, _email, _schema);

                if (!CompanyStatus.CompanyStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _companyService.UpdateStatus(status, cnpj)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar a empresa"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}