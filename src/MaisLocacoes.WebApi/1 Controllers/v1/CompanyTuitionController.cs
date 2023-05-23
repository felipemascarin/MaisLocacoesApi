using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/companytuition")]
    [ApiController]
    public class CompanyTuitionController : Controller
    {
        private readonly ICompanyTuitionService _companyTuitionService;
        private readonly IValidator<CompanyTuitionRequest> _companyTuitionValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyTuitionController(ICompanyTuitionService companyTuitionService,
            IValidator<CompanyTuitionRequest> companyTuitionValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyTuitionService = companyTuitionService;
            _companyTuitionValidator = companyTuitionValidator;
            _logger = loggerFactory.CreateLogger<CompanyTuitionController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyTuition([FromBody] CompanyTuitionRequest companyTuitionRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompanyTuition {@dateTime} {@companyTuitionRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyTuitionRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompanyTuition = _companyTuitionValidator.Validate(companyTuitionRequest);

                if (!validatedCompanyTuition.IsValid)
                {
                    var companyTuitionValidationErros = new List<string>();
                    validatedCompanyTuition.Errors.ForEach(error => companyTuitionValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyTuitionValidationErros);
                }
                return await Task.FromResult(Ok(companyTuitionRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}