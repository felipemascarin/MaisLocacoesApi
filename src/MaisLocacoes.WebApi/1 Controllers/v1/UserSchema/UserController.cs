using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices.UserSchema;

namespace MaisLocacoes.WebApi.Controllers.v1.UserSchema
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserRequest> _userValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService,
            IValidator<UserRequest> userValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _userValidator = userValidator;
            _logger = loggerFactory.CreateLogger<UserController>();
            _httpContextAccessor = httpContextAccessor;
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            try
            {
                _logger.LogInformation("CreateUser {@dateTime} {@userRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(userRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedUser = _userValidator.Validate(userRequest);

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
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailOdCpf(string email)
        {
            try
            {
                _logger.LogInformation("GetByEmail {@dateTime} email:{@email} User:{@email}", System.DateTime.Now, email, JwtManager.GetEmailByToken(_httpContextAccessor));

                var user = await _userService.GetByEmail(email);
                if (string.IsNullOrEmpty(user.Email)) return NotFound("Usuário não encontrado");
                return Ok(user);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            try
            {
                _logger.LogInformation("GetByCpf {@dateTime} cpf:{@cpf} User:{@email}", System.DateTime.Now, cpf, JwtManager.GetEmailByToken(_httpContextAccessor));

                var user = await _userService.GetByCpf(cpf);
                return Ok(user);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest, string email)
        {
            try
            {
                _logger.LogInformation("UpdateUser {@dateTime} {@userRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(userRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedUser = _userValidator.Validate(userRequest);

                if (!validatedUser.IsValid)
                {
                    var userValidationErros = new List<string>();
                    validatedUser.Errors.ForEach(error => userValidationErros.Add(error.ErrorMessage));
                    return BadRequest(userValidationErros);
                }

                if (await _userService.UpdateUser(userRequest, email)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o usuário"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPut("email/{email}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, string email)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} email:{@email} User:{@email}", System.DateTime.Now, status, email, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!UserStatus.UserStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _userService.UpdateStatus(status, email)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o usuário"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            try
            {
                _logger.LogInformation("DeleteByEmail {@dateTime} email:{@email} User:{@email}", System.DateTime.Now, email, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _userService.DeleteByEmail(email)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o usuário"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}