using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi._1Controllers.v1.TimeZone
{
    [Route("api/v1/timezone")]
    [ApiController]
    public class TimeZoneController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly string _email;

        public TimeZoneController(ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger<TimeZoneController>();
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        [Authorize(Roles = "adm")]
        [HttpGet]
        public IActionResult GetTimeZoneNames()
        {
            try
            {
                _logger.LogInformation("GetTimeZoneNames {@dateTime} User:{@email}", System.DateTime.UtcNow, _email);

                var timeZoneListNames = new List<string>();

                foreach (var timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
                {
                    string tz = TZConvert.WindowsToIana(timeZoneInfo.Id);
                    timeZoneListNames.Add(tz);
                }                    

                return Ok(timeZoneListNames);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}
