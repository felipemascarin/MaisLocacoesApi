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
    [Route("api/v1/qg")]
    [ApiController]
    public class QgController : Controller
    {
        private readonly IQgService _qgService;
        private readonly IValidator<QgRequest> _qgValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QgController(IQgService qgService,
            IValidator<QgRequest> qgValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _qgService = qgService;
            _qgValidator = qgValidator;
            _logger = loggerFactory.CreateLogger<QgController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateQg([FromBody] QgRequest qgRequest)
        {
            try
            {
                _logger.LogInformation("CreateQg {@dateTime} {@qgRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(qgRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedQg = _qgValidator.Validate(qgRequest);

                if (!validatedQg.IsValid)
                {
                    var qgValidationErros = new List<string>();
                    validatedQg.Errors.ForEach(error => qgValidationErros.Add(error.ErrorMessage));
                    return BadRequest(qgValidationErros);
                }

                var qgCreated = await _qgService.CreateQg(qgRequest);

                return CreatedAtAction(nameof(GetById), new { id = qgCreated.Id }, qgCreated);
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

                var qg = await _qgService.GetById(id);
                return Ok(qg);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll {@dateTime} User:{@email}", System.DateTime.Now, JwtManager.GetEmailByToken(_httpContextAccessor));

                var os = await _qgService.GetAll();
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
        public async Task<IActionResult> UpdateQg([FromBody] QgRequest qgRequest, int id)
        {
            try
            {
                _logger.LogInformation("Updateqg {@dateTime} {@qgRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(qgRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedQg = _qgValidator.Validate(qgRequest);

                if (!validatedQg.IsValid)
                {
                    var qgValidationErros = new List<string>();
                    validatedQg.Errors.ForEach(error => qgValidationErros.Add(error.ErrorMessage));
                    return BadRequest(qgValidationErros);
                }

                if (await _qgService.UpdateQg(qgRequest, id)) return Ok();
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

                if (await _qgService.DeleteById(id)) return Ok();
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