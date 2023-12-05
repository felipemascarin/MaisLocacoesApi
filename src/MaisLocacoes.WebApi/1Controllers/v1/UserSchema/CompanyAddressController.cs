using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi._1_Controllers.v1.UserSchema
{
    [Route("api/v1/companyaddress")]
    [ApiController]
    public class CompanyAddressController : Controller
    {
        private readonly ICompanyAddressService _companyAddressService;
        private readonly IValidator<CreateCompanyAddressRequest> _createCompanyAddressValidator;
        private readonly IValidator<UpdateCompanyAddressRequest> _updateCompanyAddressValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public CompanyAddressController(ICompanyAddressService companyAddressService,
        IValidator<CreateCompanyAddressRequest> createCompanyAddressValidator,
        IValidator<UpdateCompanyAddressRequest> updateCompanyAddressValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyAddressService = companyAddressService;
            _createCompanyAddressValidator = createCompanyAddressValidator;
            _updateCompanyAddressValidator = updateCompanyAddressValidator;
            _logger = loggerFactory.CreateLogger<CompanyAddressController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize(Roles = "adm")]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyAddress([FromBody] CreateCompanyAddressRequest request)
        {
            try
            {
                _logger.LogInformation("CreateCompanyAddress {@dateTime} {@request} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(request), _email, _cnpj);

                var validatedCompanyAddress = _createCompanyAddressValidator.Validate(request);

                if (!validatedCompanyAddress.IsValid)
                {
                    var companyAddressValidationErros = new List<string>();
                    validatedCompanyAddress.Errors.ForEach(error => companyAddressValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyAddressValidationErros);
                }

                var companyAddressCreated = await _companyAddressService.CreateCompanyAddress(request);
                return CreatedAtAction(nameof(GetById), new { id = companyAddressCreated.Id }, companyAddressCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [TokenValidationDataBase]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                var companyAddress = await _companyAddressService.GetCompanyAddressById(id);
                return Ok(companyAddress);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [TokenValidationDataBase]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyAddress([FromBody] UpdateCompanyAddressRequest request, int id)
        {
            try
            {
                _logger.LogInformation("UpdateCompanyAddress {@dateTime} {@request} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(request), id, _email, _cnpj);

                var validatedCompanyAddress = _updateCompanyAddressValidator.Validate(request);

                if (!validatedCompanyAddress.IsValid)
                {
                    var companyAddressValidationErros = new List<string>();
                    validatedCompanyAddress.Errors.ForEach(error => companyAddressValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyAddressValidationErros);
                }

                await _companyAddressService.UpdateCompanyAddress(request, id);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //Não possui delete
    }
}