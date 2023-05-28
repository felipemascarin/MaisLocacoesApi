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
    [Route("api/v1/os")]
    [ApiController]
    public class OsController : Controller
    {
        private readonly IOsService _osService;
        private readonly IValidator<OsRequest> _osValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OsController(IOsService osService,
            IValidator<OsRequest> osValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _osService = osService;
            _osValidator = osValidator;
            _logger = loggerFactory.CreateLogger<OsController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateOs([FromBody] OsRequest osRequest)
        {
            try
            {
                _logger.LogInformation("CreateOs {@dateTime} {@osRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(osRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedOs = _osValidator.Validate(osRequest);

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

                var os = await _osService.GetById(id);
                return Ok(os);
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
        public async Task<IActionResult> Updateos([FromBody]OsRequest osRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateOs {@dateTime} {@osRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(osRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedOs = _osValidator.Validate(osRequest);

                if (!validatedOs.IsValid)
                {
                    var osValidationErros = new List<string>();
                    validatedOs.Errors.ForEach(error => osValidationErros.Add(error.ErrorMessage));
                    return BadRequest(osValidationErros);
                }

                if (await _osService.UpdateOs(osRequest, id)) return Ok();
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

                if (!OsStatus.OsStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _osService.UpdateStatus(status, id)) return Ok();
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

                if (await _osService.DeleteById(id)) return Ok();
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