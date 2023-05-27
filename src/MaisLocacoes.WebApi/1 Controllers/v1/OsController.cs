using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using Service.v1.Services;

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
                return await Task.FromResult(Ok(osRequest));
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