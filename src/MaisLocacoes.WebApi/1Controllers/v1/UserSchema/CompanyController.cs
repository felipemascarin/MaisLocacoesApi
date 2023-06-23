using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
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
        private readonly IValidator<CompanyRequest> _companyValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyController(ICompanyService companyService,
            IValidator<CompanyRequest> companyValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _companyValidator = companyValidator;
            _logger = loggerFactory.CreateLogger<CompanyController>();
            _httpContextAccessor = httpContextAccessor;
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRequest companyRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompany {@dateTime} {@companyRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompany = _companyValidator.Validate(companyRequest);

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
                _logger.LogInformation("GetByCnpj {@dateTime} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                var company = await _companyService.GetByCnpj(cnpj);
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
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyRequest companyRequest, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateCompany {@dateTime} {@companyRequest} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyRequest), cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompany = _companyValidator.Validate(companyRequest);

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
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, status, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

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