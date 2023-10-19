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
        private readonly IValidator<CreateCompanyWasteRequest> _createCompanyWasteValidator;
        private readonly IValidator<UpdateCompanyWasteRequest> _updateCompanyWasteValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public CompanyWasteController(ICompanyWasteService companyWasteService,
            IValidator<CreateCompanyWasteRequest> createCompanyWasteValidator,
            IValidator<UpdateCompanyWasteRequest> updateCompanyWasteValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyWasteService = companyWasteService;
            _createCompanyWasteValidator = createCompanyWasteValidator;
            _updateCompanyWasteValidator = updateCompanyWasteValidator;
            _logger = loggerFactory.CreateLogger<CompanyWasteController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyWaste([FromBody] CreateCompanyWasteRequest companyWasteRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompanyWaste {@dateTime} {@companyWasteRequest} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(companyWasteRequest), _email, _schema);

                var validatedCompanyWaste = _createCompanyWasteValidator.Validate(companyWasteRequest);

                if (!validatedCompanyWaste.IsValid)
                {
                    var companyWasteValidationErros = new List<string>();
                    validatedCompanyWaste.Errors.ForEach(error => companyWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyWasteValidationErros);
                }

                var companyWasteCreated = await _companyWasteService.CreateCompanyWaste(companyWasteRequest);

                return CreatedAtAction(nameof(GetById), new { id = companyWasteCreated.Id }, companyWasteCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

                var companyWaste = await _companyWasteService.GetCompanyWasteById(id);
                return Ok(companyWaste);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyWaste([FromBody] UpdateCompanyWasteRequest companyWasteRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateCompanyWaste {@dateTime} {@companyWasteRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(companyWasteRequest), id, _email, _schema);

                var validatedCompanyWaste = _updateCompanyWasteValidator.Validate(companyWasteRequest);

                if (!validatedCompanyWaste.IsValid)
                {
                    var companyWasteValidationErros = new List<string>();
                    validatedCompanyWaste.Errors.ForEach(error => companyWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyWasteValidationErros);
                }

                if (await _companyWasteService.UpdateCompanyWaste(companyWasteRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

                if (await _companyWasteService.DeleteById(id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível deletar"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}