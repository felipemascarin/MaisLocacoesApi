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
    [Route("api/v1/rentedplace")]
    [ApiController]
    public class RentedPlaceController : Controller
    {
        private readonly IRentedPlaceService _rentedPlaceService;
        private readonly IValidator<RentedPlaceRequest> _rentedPlaceValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentedPlaceController(IRentedPlaceService rentedPlaceService,
            IValidator<RentedPlaceRequest> rentedPlaceValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _rentedPlaceService = rentedPlaceService;
            _rentedPlaceValidator = rentedPlaceValidator;
            _logger = loggerFactory.CreateLogger<RentedPlaceController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateRentedPlace([FromBody] RentedPlaceRequest rentedPlaceRequest)
        {
            try
            {
                _logger.LogInformation("CreateRentedPlace {@dateTime} {@rentedPlaceRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(rentedPlaceRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedRentedPlace = _rentedPlaceValidator.Validate(rentedPlaceRequest);

                if (!validatedRentedPlace.IsValid)
                {
                    var rentedPlaceValidationErros = new List<string>();
                    validatedRentedPlace.Errors.ForEach(error => rentedPlaceValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentedPlaceValidationErros);
                }
                return await Task.FromResult(Ok(rentedPlaceRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}