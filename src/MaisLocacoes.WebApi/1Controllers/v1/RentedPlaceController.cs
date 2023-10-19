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
        private readonly IValidator<CreateRentedPlaceRequest> _createRentedPlaceValidator;
        private readonly IValidator<UpdateRentedPlaceRequest> _updateRentedPlaceValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentedPlaceController(IRentedPlaceService rentedPlaceService,
            IValidator<CreateRentedPlaceRequest> createRentedPlaceValidator,
            IValidator<UpdateRentedPlaceRequest> updateRentedPlaceValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _rentedPlaceService = rentedPlaceService;
            _createRentedPlaceValidator = createRentedPlaceValidator;
            _updateRentedPlaceValidator = updateRentedPlaceValidator;
            _logger = loggerFactory.CreateLogger<RentedPlaceController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateRentedPlace([FromBody] CreateRentedPlaceRequest rentedPlaceRequest)
        {
            try
            {
                _logger.LogInformation("CreateRentedPlace {@dateTime} {@rentedPlaceRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(rentedPlaceRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedRentedPlace = _createRentedPlaceValidator.Validate(rentedPlaceRequest);

                if (!validatedRentedPlace.IsValid)
                {
                    var rentedPlaceValidationErros = new List<string>();
                    validatedRentedPlace.Errors.ForEach(error => rentedPlaceValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentedPlaceValidationErros);
                }

                var rentedPlaceCreated = await _rentedPlaceService.CreateRentedPlace(rentedPlaceRequest);

                return CreatedAtAction(nameof(GetById), new { id = rentedPlaceCreated.Id }, rentedPlaceCreated);
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

                var rentedPlace = await _rentedPlaceService.GetRentedPlaceById(id);
                return Ok(rentedPlace);
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
        public async Task<IActionResult> UpdateRentedPlace([FromBody] UpdateRentedPlaceRequest rentedPlaceRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdaterentedPlace {@dateTime} {@rentedPlaceRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(rentedPlaceRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedRentedPlace = _updateRentedPlaceValidator.Validate(rentedPlaceRequest);

                if (!validatedRentedPlace.IsValid)
                {
                    var rentedPlaceValidationErros = new List<string>();
                    validatedRentedPlace.Errors.ForEach(error => rentedPlaceValidationErros.Add(error.ErrorMessage));
                    return BadRequest(rentedPlaceValidationErros);
                }

                if (await _rentedPlaceService.UpdateRentedPlace(rentedPlaceRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}