using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
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
    [Route("api/v1/os")]
    [ApiController]
    public class OsController : Controller
    {
        private readonly IOsService _osService;
        private readonly IValidator<CreateOsRequest> _createOsValidator;
        private readonly IValidator<UpdateOsRequest> _updateOsValidator;
        private readonly IValidator<FinishOsRequest> _finishOsValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public OsController(IOsService osService,
            IValidator<CreateOsRequest> createOsValidator,
            IValidator<UpdateOsRequest> updateOsValidator,
            IValidator<FinishOsRequest> finishOsValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _osService = osService;
            _createOsValidator = createOsValidator;
            _updateOsValidator = updateOsValidator;
            _finishOsValidator = finishOsValidator;
            _logger = loggerFactory.CreateLogger<OsController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateOs([FromBody] CreateOsRequest osRequest)
        {
            try
            {
                _logger.LogInformation("CreateOs {@dateTime} {@osRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(osRequest), _email, _cnpj);

                var validatedOs = _createOsValidator.Validate(osRequest);

                if (!validatedOs.IsValid)
                {
                    var osValidationErros = new List<string>();
                    validatedOs.Errors.ForEach(error => osValidationErros.Add(error.ErrorMessage));
                    return BadRequest(osValidationErros);
                }

                var osCreated = await _osService.CreateOs(osRequest);

                return CreatedAtAction(nameof(GetById), new { id = osCreated.Id }, osCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost("start/{id}")]
        public async Task<IActionResult> StartOs(int id)
        {
            try
            {
                _logger.LogInformation("StartOs {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _osService.StartOs(id);

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
        [HttpPost("return/{id}")]
        public async Task<IActionResult> ReturnOs(int id)
        {
            try
            {
                _logger.LogInformation("ReturnOs {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _osService.ReturnOs(id);

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
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelOs(int id)
        {
            try
            {
                _logger.LogInformation("CancelOs {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _osService.CancelOs(id);

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
        [HttpPost("finish/{id}")]
        public async Task<IActionResult> FinishOs(int id, [FromBody] FinishOsRequest closeOsRequest)
        {
            try
            {
                _logger.LogInformation("FinishOs {@dateTime} id:{@id} request:{@request} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, JsonConvert.SerializeObject(closeOsRequest), _email, _cnpj);

                var validatedOs = _finishOsValidator.Validate(closeOsRequest);

                if (!validatedOs.IsValid)
                {
                    var osValidationErros = new List<string>();
                    validatedOs.Errors.ForEach(error => osValidationErros.Add(error.ErrorMessage));
                    return BadRequest(osValidationErros);
                }

                await _osService.FinishOs(id, closeOsRequest);

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                var os = await _osService.GetOsById(id);
                return Ok(os);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet]
        public async Task<IActionResult> GetAllByStatus([FromQuery(Name = "status")] string status)
        {
            try
            {
                _logger.LogInformation("GetAllByStatus {@dateTime} status:{@status} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), status, _email, _cnpj);

                if (status != null && !OsStatus.OsStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                var os = await _osService.GetAllOsByStatus(status);
                return Ok(os);
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
        public async Task<IActionResult> UpdateOs([FromBody] UpdateOsRequest osRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateOs {@dateTime} {@osRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(osRequest), id, _email, _cnpj);

                var validatedOs = _updateOsValidator.Validate(osRequest);

                if (!validatedOs.IsValid)
                {
                    var osValidationErros = new List<string>();
                    validatedOs.Errors.ForEach(error => osValidationErros.Add(error.ErrorMessage));
                    return BadRequest(osValidationErros);
                }

                await _osService.UpdateOs(osRequest, id);

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

                if (!OsStatus.OsStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                await _osService.UpdateStatus(status, id);

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

                await _osService.DeleteById(id);

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