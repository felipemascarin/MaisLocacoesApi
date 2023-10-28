using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using System;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/address")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IValidator<CreateAddressRequest> _createAddressValidator;
        private readonly IValidator<UpdateAddressRequest> _updateAddressValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public AddressController(IAddressService addressService,
            IValidator<CreateAddressRequest> createAddressValidator,
            IValidator<UpdateAddressRequest> updateAddressValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _addressService = addressService;
            _createAddressValidator = createAddressValidator;
            _updateAddressValidator = updateAddressValidator;
            _logger = loggerFactory.CreateLogger<AddressController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest address)
        {
            try
            {
                _logger.LogInformation("CreateAddress {@dateTime} {@address} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(address), _email, _schema);

                var validatedAddress = _createAddressValidator.Validate(address);

                if (!validatedAddress.IsValid)
                {
                    var addressValidationErros = new List<string>();
                    validatedAddress.Errors.ForEach(error => addressValidationErros.Add(error.ErrorMessage));
                    return BadRequest(addressValidationErros);
                }

                var addressCreated = await _addressService.CreateAddress(address);
                return CreatedAtAction(nameof(GetById), new { id = addressCreated.Id }, addressCreated);
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

                var address = await _addressService.GetAddressById(id);
                return Ok(address);
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
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest addressRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateAddress {@dateTime} {@addressRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(addressRequest), id, _email, _schema);

                var validatedAddress = _updateAddressValidator.Validate(addressRequest);

                if (!validatedAddress.IsValid)
                {
                    var addressValidationErros = new List<string>();
                    validatedAddress.Errors.ForEach(error => addressValidationErros.Add(error.ErrorMessage));
                    return BadRequest(addressValidationErros);
                }

                await _addressService.UpdateAddress(addressRequest, id);

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