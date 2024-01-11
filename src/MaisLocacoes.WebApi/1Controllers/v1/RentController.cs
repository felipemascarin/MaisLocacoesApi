using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/rent")]
    [ApiController]
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly IValidator<CreateRentRequest> _createRentValidator;
        private readonly IValidator<UpdateRentRequest> _updateRentValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public RentController(IRentService rentService,
            IValidator<CreateRentRequest> createRentValidator,
            IValidator<UpdateRentRequest> updateRentValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentService = rentService;
            _createRentValidator = createRentValidator;
            _updateRentValidator = updateRentValidator;
            _logger = loggerFactory.CreateLogger<RentController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateRent([FromBody] CreateRentRequest rentRequest)
        {
            try
            {
                _logger.LogInformation("CreateRent {@dateTime} {@rentRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(rentRequest), _email, _cnpj);

                var validatedRent = _createRentValidator.Validate(rentRequest);

                if (!validatedRent.IsValid)
                {
                    var rentValidationErros = new List<string>();
                    validatedRent.Errors.ForEach(error => rentValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentValidationErros);
                }

                var rentCreated = await _rentService.CreateRent(rentRequest);

                return CreatedAtAction(nameof(GetById), new { id = rentCreated.Id }, rentCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                var rent = await _rentService.GetRentById(id);
                return Ok(rent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetAllByClientId(int clientId)
        {
            try
            {
                _logger.LogInformation("GetAllByClientId {@dateTime} clientId:{@clientId} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), clientId, _email, _cnpj);

                var rents = await _rentService.GetAllRentsByClientId(clientId);
                return Ok(rents);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("items/{items}/page/{page}")]
        public async Task<IActionResult> GetRentsByPage(int items, int page, [FromQuery(Name = "query")] string query, [FromQuery(Name = "status")] string status)
        {
            try
            {
                _logger.LogInformation("GetRentsByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), items, page, query, _email, _cnpj);

                var rentsList = await _rentService.GetRentsByPage(items, page, query, status);
                return Ok(rentsList);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRent([FromBody] UpdateRentRequest rentRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateRent {@dateTime} {@rentRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(rentRequest), id, _email, _cnpj);

                var validatedRent = _updateRentValidator.Validate(rentRequest);

                if (!validatedRent.IsValid)
                {
                    var rentValidationErros = new List<string>();
                    validatedRent.Errors.ForEach(error => rentValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentValidationErros);
                }

                await _rentService.UpdateRent(rentRequest, id);
                
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), status, id, _email, _cnpj);

                if (!RentStatus.RentStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                await _rentService.UpdateStatus(status, id);
                
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _rentService.DeleteById(id);
                
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