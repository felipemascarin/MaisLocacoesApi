using FluentValidation;
using MaisLocacoes.WebApi.Controllers.v1.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using System.Net;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/address")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IValidator<AddressRequest> _addressValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddressController(IAddressService addressService,
            IValidator<AddressRequest> addressValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _addressService = addressService;
            _addressValidator = addressValidator;
            _logger = loggerFactory.CreateLogger<AddressController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressRequest address)
        {
            try
            {
                _logger.LogInformation("CreateAddress {@dateTime} {@address} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(address), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedAddress = _addressValidator.Validate(address);

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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var address = await _addressService.GetById(id);
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
        public async Task<IActionResult> UpdateAddress([FromBody] AddressRequest addressRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateAddress {@dateTime} {@addressRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(addressRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedAddress = _addressValidator.Validate(addressRequest);

                if (!validatedAddress.IsValid)
                {
                    var addressValidationErros = new List<string>();
                    validatedAddress.Errors.ForEach(error => addressValidationErros.Add(error.ErrorMessage));
                    return BadRequest(addressValidationErros);
                }

                if (await _addressService.UpdateAddress(addressRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o endereço"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}