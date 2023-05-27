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

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/client")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IValidator<ClientRequest> _clientValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientController(IClientService clientService,
            IValidator<ClientRequest> clientValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientService = clientService;
            _clientValidator = clientValidator;
            _logger = loggerFactory.CreateLogger<ClientController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientRequest clientRequest)
        {
            try
            {
                _logger.LogInformation("CreateClient {@dateTime} {@clientRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(clientRequest), JwtManager.GetEmailByToken(_httpContextAccessor));
                
                var validatedClient = _clientValidator.Validate(clientRequest);

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var client = await _clientService.GetById(id);
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
                _logger.LogInformation("GetClientsByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email}", System.DateTime.Now, items, page, query, JwtManager.GetEmailByToken(_httpContextAccessor));

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
                _logger.LogInformation("GetClientsForRent {@dateTime} User:{@email}", System.DateTime.Now, JwtManager.GetEmailByToken(_httpContextAccessor));

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
        public async Task<IActionResult> UpdateClient([FromBody] ClientRequest clientRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateClient {@dateTime} {@clientRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(clientRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedClient = _clientValidator.Validate(clientRequest);

                if (!validatedClient.IsValid)
                {
                    var clientValidationErros = new List<string>();
                    validatedClient.Errors.ForEach(error => clientValidationErros.Add(error.ErrorMessage));
                    return BadRequest(clientValidationErros);
                }

                if (await _clientService.UpdateClient(clientRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o cliente"));
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
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email}", System.DateTime.Now, status, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!ClientStatus.ClientStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _clientService.UpdateStatus(status, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o cliente"));
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _clientService.DeleteById(id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível deletar a fatura"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}