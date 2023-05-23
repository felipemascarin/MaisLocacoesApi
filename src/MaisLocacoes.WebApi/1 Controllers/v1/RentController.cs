using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
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

                //Verificar se terá mudança no status devido a data (agendada, alugada)

                var validatedRent = _rentValidator.Validate(rentRequest);

                if (!validatedRent.IsValid)
                {
                    var rentValidationErros = new List<string>();
                    validatedRent.Errors.ForEach(error => rentValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentValidationErros);
                }
                return await Task.FromResult(Ok(rentRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}