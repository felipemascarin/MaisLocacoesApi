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
    [Route("api/v1/companywaste")]
    [ApiController]
    public class CompanyWasteController : Controller
    {
        private readonly ICompanyWasteService _companyWasteService;
        private readonly IValidator<CompanyWasteRequest> _companyWasteValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyWasteController(ICompanyWasteService companyWasteService,
            IValidator<CompanyWasteRequest> companyWasteValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyWasteService = companyWasteService;
            _companyWasteValidator = companyWasteValidator;
            _logger = loggerFactory.CreateLogger<CompanyWasteController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyWaste([FromBody] CompanyWasteRequest companyWasteRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompanyWaste {@dateTime} {@companyWasteRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyWasteRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompanyWaste = _companyWasteValidator.Validate(companyWasteRequest);

                if (!validatedCompanyWaste.IsValid)
                {
                    var companyWasteValidationErros = new List<string>();
                    validatedCompanyWaste.Errors.ForEach(error => companyWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyWasteValidationErros);
                }
                return await Task.FromResult(Ok(companyWasteRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}