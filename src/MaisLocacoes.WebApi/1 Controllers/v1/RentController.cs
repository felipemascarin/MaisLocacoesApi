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

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/rent")]
    [ApiController]
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly IValidator<RentRequest> _rentValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentController(IRentService rentService,
            IValidator<RentRequest> rentValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentService = rentService;
            _rentValidator = rentValidator;
            _logger = loggerFactory.CreateLogger<RentController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateRent([FromBody] RentRequest rentRequest)
        {
            try
            {
                _logger.LogInformation("CreateRent {@dateTime} {@rentRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(rentRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedRent = _rentValidator.Validate(rentRequest);

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

                var rent = await _rentService.GetById(id);
                return Ok(rent);
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
        public async Task<IActionResult> GetRentsByPage(int items, int page, [FromQuery(Name = "query")] string query)
        {
            try
            {
                _logger.LogInformation("GetRentsByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email}", System.DateTime.Now, items, page, query, JwtManager.GetEmailByToken(_httpContextAccessor));

                var rentsList = await _rentService.GetRentsByPage(items, page, query);
                return Ok(rentsList);
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
        public async Task<IActionResult> UpdateRent([FromBody] RentRequest rentRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateRent {@dateTime} {@rentRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(rentRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedRent = _rentValidator.Validate(rentRequest);

                if (!validatedRent.IsValid)
                {
                    var rentValidationErros = new List<string>();
                    validatedRent.Errors.ForEach(error => rentValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentValidationErros);
                }

                if (await _rentService.UpdateRent(rentRequest, id)) return Ok();
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
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email}", System.DateTime.Now, status, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!RentStatus.RentStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _rentService.UpdateStatus(status, id)) return Ok();
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _rentService.DeleteById(id)) return Ok();
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