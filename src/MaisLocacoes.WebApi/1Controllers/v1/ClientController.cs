using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.v1.IServices;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/client")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IValidator<CreateClientRequest> _createClientValidator;
        private readonly IValidator<UpdateClientRequest> _updateClientValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public ClientController(IClientService clientService,
            IValidator<CreateClientRequest> createClientValidator,
            IValidator<UpdateClientRequest> updateClientValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientService = clientService;
            _createClientValidator = createClientValidator;
            _updateClientValidator = updateClientValidator;
            _logger = loggerFactory.CreateLogger<ClientController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest clientRequest)
        {
            try
            {
                _logger.LogInformation("CreateClient {@dateTime} {@clientRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(clientRequest), _email, _schema);

                var validatedClient = _createClientValidator.Validate(clientRequest);

                if (!validatedClient.IsValid)
                {
                    var clientValidationErros = new List<string>();
                    validatedClient.Errors.ForEach(error => clientValidationErros.Add(error.ErrorMessage));
                    return BadRequest(clientValidationErros);
                }

                var clientCreated = await _clientService.CreateClient(clientRequest);

                return CreatedAtAction(nameof(GetById), new { id = clientCreated.Id }, clientCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

                var client = await _clientService.GetClientById(id);
                return Ok(client);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("details/id/{id}")]
        public async Task<IActionResult> GetByIdDetails(int id)
        {
            try
            {
                _logger.LogInformation("GetByIdDetails {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

                var client = await _clientService.GetClientByIdDetails(id);
                return Ok(client);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            try
            {
                _logger.LogInformation("GetBycpf {@dateTime} cpf:{@cpf} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), cpf, _email, _schema);

                var client = await _clientService.GetClientByCpf(cpf);
                return Ok(client);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("cnpj/{cnpj}")]
        public async Task<IActionResult> GetByCnpj(string cnpj)
        {
            try
            {
                _logger.LogInformation("GetBycnpj {@dateTime} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), cnpj, _email, _schema);

                var client = await _clientService.GetClientByCnpj(cnpj);
                return Ok(client);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("items/{items}/page/{page}")]
        public async Task<IActionResult> GetClientsByPage(int items, int page, [FromQuery(Name = "query")] string query)
        {
            try
            {
                _logger.LogInformation("GetClientsByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), items, page, query, _email, _schema);

                var clientsList = await _clientService.GetClientsByPage(items, page, query);
                return Ok(clientsList);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("rent")]
        public async Task<IActionResult> GetClientsForRent()
        {
            try
            {
                _logger.LogInformation("GetClientsForRent {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _schema);

                var clientsList = await _clientService.GetClientsForRent();
                return Ok(clientsList);
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
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequest clientRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateClient {@dateTime} {@clientRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(clientRequest), id, _email, _schema);

                var validatedClient = _updateClientValidator.Validate(clientRequest);

                if (!validatedClient.IsValid)
                {
                    var clientValidationErros = new List<string>();
                    validatedClient.Errors.ForEach(error => clientValidationErros.Add(error.ErrorMessage));
                    return BadRequest(clientValidationErros);
                }

                await _clientService.UpdateClient(clientRequest, id);

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
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), status, id, _email, _schema);

                if (!ClientStatus.ClientStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                await _clientService.UpdateStatus(status, id);

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

                await _clientService.DeleteById(id);

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