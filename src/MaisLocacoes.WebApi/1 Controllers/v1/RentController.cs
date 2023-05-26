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
    }
}