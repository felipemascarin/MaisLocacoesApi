using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices.UserSchema;
using System.Data;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1.UserSchema
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public UserController(IUserService userService,
            IValidator<CreateUserRequest> createUserValidator,
            IValidator<UpdateUserRequest> updateUserValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _logger = loggerFactory.CreateLogger<UserController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize(Roles = "adm")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
        {
            try
            {
                _logger.LogInformation("CreateUser {@dateTime} {@userRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(userRequest), _email, _cnpj);

                var validatedUser = _createUserValidator.Validate(userRequest);

                if (!validatedUser.IsValid)
                {
                    var userValidationErros = new List<string>();
                    validatedUser.Errors.ForEach(error => userValidationErros.Add(error.ErrorMessage));
                    return BadRequest(userValidationErros);
                }

                var userCreated = await _userService.CreateUser(userRequest);

                return Ok(userCreated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpGet("email/{email}/cnpj/{cnpj}")]
        public async Task<IActionResult> GetByEmail(string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByEmail {@dateTime} email:{@email} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), email, cnpj, _email, _cnpj);

                var user = await _userService.GetUserByEmail(email, cnpj);
                if (string.IsNullOrEmpty(user.Email)) return NotFound("Usuário não encontrado");
                return Ok(user);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpGet("cpf/{cpf}/cnpj/{cnpj}")]
        public async Task<IActionResult> GetByCpf(string cpf, string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByCpf {@dateTime} cpf:{@cpf} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), cpf, cnpj, _email, _cnpj);

                var user = await _userService.GetUserByCpf(cpf, cnpj);
                return Ok(user);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpGet("cnpj/{cnpj}")]
        public async Task<IActionResult> GetAllByCnpj(string cnpj)
        {
            try
            {
                _logger.LogInformation("GetAllByCnpj {@dateTime} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), cnpj, _email, _cnpj);

                var os = await _userService.GetAllUsersByCnpj(cnpj);
                return Ok(os);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpPut("email/{email}/cnpj/{cnpj}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest userRequest, string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateUser {@dateTime} {@userRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(userRequest), _email, _cnpj);

                var validatedUser = _updateUserValidator.Validate(userRequest);

                if (!validatedUser.IsValid)
                {
                    var userValidationErros = new List<string>();
                    validatedUser.Errors.ForEach(error => userValidationErros.Add(error.ErrorMessage));
                    return BadRequest(userValidationErros);
                }

                await _userService.UpdateUser(userRequest, email, cnpj);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize(Roles = "adm")]
        [HttpPut("email/{email}/cnpj/{cnpj}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} email:{@email} cnpj:{@cnpj} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), status, email, cnpj, _email, _cnpj);

                if (!UserStatus.UserStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                await _userService.UpdateStatus(status, email, cnpj);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}