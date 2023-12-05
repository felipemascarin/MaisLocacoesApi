using MaisLocacoes.WebApi._2Service.v1.IServices.Migration;
using MaisLocacoes.WebApi.Controllers.v1.UserSchema;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaisLocacoes.WebApi._1Controllers.v1.Migration
{
    [Route("api/v1/migration")]
    [ApiController]
    public class MigrationController : Controller
    {
        private readonly IMigrationService _migrationService;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _email;
        private readonly string _cnpj;

        public MigrationController(IMigrationService migrationService,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _migrationService = migrationService;
            _logger = loggerFactory.CreateLogger<CompanyController>();
            _httpContextAccessor = httpContextAccessor;
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize(Roles = "adm")]
        [HttpPut]
        public async Task<IActionResult> AddMigrationAllDataBases()
        {
            try
            {
                _logger.LogInformation("AddMigrationAllDataBases {@dateTime} UTC User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow, _email, _cnpj);

                await _migrationService.AddMigrationAllDataBases();
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
