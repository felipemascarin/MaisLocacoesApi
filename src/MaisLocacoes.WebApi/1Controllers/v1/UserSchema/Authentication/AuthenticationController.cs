using FluentValidation;
using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidator<LoginRequest> _loginRequestValidator;
        private readonly IValidator<LogoutRequest> _logoutRequestValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthenticationService authenticationService,
            IValidator<LoginRequest> loginRequestValidator,
            IValidator<LogoutRequest> logoutRequestValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _loginRequestValidator = loginRequestValidator;
            _logoutRequestValidator = logoutRequestValidator;
            _logger = loggerFactory.CreateLogger<AuthenticationController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login {@dateTime} {@request}", System.DateTime.Now, JsonConvert.SerializeObject(request));

                var validatedLogin = _loginRequestValidator.Validate(request);

                if (!validatedLogin.IsValid)
                {
                    var loginValidationErros = new List<string>();
                    validatedLogin.Errors.ForEach(error => loginValidationErros.Add(error.ErrorMessage));
                    return BadRequest(loginValidationErros);
                }

                //Verificar se o token recebido no request é um token válido no firebase:
                //if (!await FireBaseAuthentication.IsFirebaseTokenValid(request.GoogleToken))
                    //return Unauthorized();

                return Ok(await _authenticationService.Login(request));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            try
            {
                _logger.LogInformation("Logout {@dateTime} {@request}", System.DateTime.Now, JsonConvert.SerializeObject(request));

                var validatedLogout = _logoutRequestValidator.Validate(request);

                if (!validatedLogout.IsValid)
                {
                    var logoutValidationErros = new List<string>();
                    validatedLogout.Errors.ForEach(error => logoutValidationErros.Add(error.ErrorMessage));
                    return BadRequest(logoutValidationErros);
                }

                if (await _authenticationService.Logout(request)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o usuário para deslogado"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost("isvalidtoken")]
        public IActionResult IsValidToken()
        {
            try
            {
                _logger.LogInformation("IsValidToken {@dateTime} User:{@email}", System.DateTime.Now, JwtManager.GetEmailByToken(_httpContextAccessor));

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}