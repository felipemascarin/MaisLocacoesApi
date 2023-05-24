using FluentValidation;
using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Infrastructure;
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
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<TokenRequest> _tokenRequestValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthenticationService authenticationService,
            IValidator<LoginRequest> loginValidator,
            IValidator<TokenRequest> tokenRequestValidator,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _loginValidator = loginValidator;
            _tokenRequestValidator = tokenRequestValidator;
            _logger = loggerFactory.CreateLogger<AuthenticationController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Login {@dateTime} {@loginRequest}", System.DateTime.Now, JsonConvert.SerializeObject(loginRequest));

                var validatedLogin = _loginValidator.Validate(loginRequest);

                if (!validatedLogin.IsValid)
                {
                    var loginValidationErros = new List<string>();
                    validatedLogin.Errors.ForEach(error => loginValidationErros.Add(error.ErrorMessage));
                    return BadRequest(loginValidationErros);
                }

                //if (!await FireBaseAuthentication.IsFirebaseTokenValid(loginRequest.GoogleToken))
                    //return Forbid();

                return Ok(await _authenticationService.Login(loginRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                _logger.LogInformation("Logout {@dateTime} {@tokenRequest}", System.DateTime.Now, JsonConvert.SerializeObject(tokenRequest));

                var validatedLogout = _tokenRequestValidator.Validate(tokenRequest);

                if (!validatedLogout.IsValid)
                {
                    var logoutValidationErros = new List<string>();
                    validatedLogout.Errors.ForEach(error => logoutValidationErros.Add(error.ErrorMessage));
                    return BadRequest(logoutValidationErros);
                }

                if (await _authenticationService.Logout(tokenRequest)) return Ok();
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