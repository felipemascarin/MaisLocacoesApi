using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
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
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

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
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
        {
            try
            {
                _logger.LogInformation("CreateUser {@dateTime} {@userRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(userRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

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
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpGet("email/{email}/cnpj/{cnpj}")]
        public async Task<IActionResult> GetByEmail(string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByEmail {@dateTime} email:{@email} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, email, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                var user = await _userService.GetUserByEmail(email, cnpj);
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
        [HttpGet("cpf/{cpf}/cnpj/{cnpj}")]
        public async Task<IActionResult> GetByCpf(string cpf, string cnpj)
        {
            try
            {
                _logger.LogInformation("GetByCpf {@dateTime} cpf:{@cpf} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, cpf, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                var user = await _userService.GetUserByCpf(cpf, cnpj);
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
        [HttpGet("cnpj/{cnpj}")]
        public async Task<IActionResult> GetAllByCnpj(string cnpj)
        {
            try
            {
                _logger.LogInformation("GetAllByCnpj {@dateTime} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                var os = await _userService.GetAllUsersByCnpj(cnpj);
                return Ok(os);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        [HttpPut("email/{email}/cnpj/{cnpj}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest userRequest, string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateUser {@dateTime} {@userRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(userRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedUser = _updateUserValidator.Validate(userRequest);

                if (!validatedUser.IsValid)
                {
                    var userValidationErros = new List<string>();
                    validatedUser.Errors.ForEach(error => userValidationErros.Add(error.ErrorMessage));
                    return BadRequest(userValidationErros);
                }

                if (await _userService.UpdateUser(userRequest, email, cnpj)) return Ok();
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
        [HttpPut("email/{email}/cnpj/{cnpj}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, string email, string cnpj)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} email:{@email} cnpj:{@cnpj} User:{@email}", System.DateTime.Now, status, email, cnpj, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!UserStatus.UserStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _userService.UpdateStatus(status, email, cnpj)) return Ok();
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