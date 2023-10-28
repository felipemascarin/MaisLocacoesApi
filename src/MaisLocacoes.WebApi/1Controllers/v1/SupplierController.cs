using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/supplier")]
    [ApiController]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IValidator<CreateSupplierRequest> _createSupplierValidator;
        private readonly IValidator<UpdateSupplierRequest> _updateSupplierValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public SupplierController(ISupplierService supplierService,
            IValidator<CreateSupplierRequest> createSupplierValidator,
            IValidator<UpdateSupplierRequest> updateSupplierValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _supplierService = supplierService;
            _createSupplierValidator = createSupplierValidator;
            _updateSupplierValidator = updateSupplierValidator;
            _logger = loggerFactory.CreateLogger<SupplierController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest supplierRequest)
        {
            try
            {
                _logger.LogInformation("CreateSupplier {@dateTime} {@supplierRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(supplierRequest), _email, _schema);

                var validatedSupplier = _createSupplierValidator.Validate(supplierRequest);

                if (!validatedSupplier.IsValid)
                {
                    var supplierValidationErros = new List<string>();
                    validatedSupplier.Errors.ForEach(error => supplierValidationErros.Add(error.ErrorMessage));
                    return BadRequest(supplierValidationErros);
                }

                var supplierCreated = await _supplierService.CreateSupplier(supplierRequest);

                return CreatedAtAction(nameof(GetById), new { id = supplierCreated.Id }, supplierCreated);
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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

                var supplier = await _supplierService.GetSupplierById(id);
                return Ok(supplier);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _schema);

                var suppliers = await _supplierService.GetAllSuppliers();
                return Ok(suppliers);
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
        public async Task<IActionResult> UpdateSupplier([FromBody] UpdateSupplierRequest supplierRequest, int id)
        {
            try
            {
                _logger.LogInformation("Updatesupplier {@dateTime} {@supplierRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(supplierRequest), id, _email, _schema);

                var validatedSupplier = _updateSupplierValidator.Validate(supplierRequest);

                if (!validatedSupplier.IsValid)
                {
                    var supplierValidationErros = new List<string>();
                    validatedSupplier.Errors.ForEach(error => supplierValidationErros.Add(error.ErrorMessage));
                    return BadRequest(supplierValidationErros);
                }

                await _supplierService.UpdateSupplier(supplierRequest, id);

                return Ok();
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

                await _supplierService.DeleteById(id);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}