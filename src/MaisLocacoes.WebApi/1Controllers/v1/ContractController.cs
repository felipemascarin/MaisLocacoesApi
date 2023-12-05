using FluentValidation;
using MaisLocacoes.WebApi._2Service.v1.IServices;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi._1Controllers.v1
{
    [Route("api/v1/contract")]
    [ApiController]
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IValidator<CreateContractRequest> _createContractValidator;
        private readonly IValidator<UpdateContractRequest> _updateContractValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public ContractController(IContractService contractService,
            IValidator<CreateContractRequest> createContractValidator,
            IValidator<UpdateContractRequest> updateContractValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _contractService = contractService;
            _createContractValidator = createContractValidator;
            _updateContractValidator = updateContractValidator;
            _logger = loggerFactory.CreateLogger<ContractController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] CreateContractRequest contractRequest)
        {
            try
            {
                _logger.LogInformation("CreateContract {@dateTime} {@contractRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(contractRequest), _email, _cnpj);

                var validatedContract = _createContractValidator.Validate(contractRequest);

                if (!validatedContract.IsValid)
                {
                    var contractValidationErros = new List<string>();
                    validatedContract.Errors.ForEach(error => contractValidationErros.Add(error.ErrorMessage));
                    return BadRequest(contractValidationErros);
                }

                var contractCreated = await _contractService.CreateContract(contractRequest);

                return CreatedAtAction(nameof(GetById), new { id = contractCreated.Id }, contractCreated);
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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                var contract = await _contractService.GetContractById(id);
                return Ok(contract);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _cnpj);

                var contracts = await _contractService.GetAllContracts();
                return Ok(contracts);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("info/rent/{rentId}")]
        public async Task<IActionResult> GetContractInfoByRentId(int rentId)
        {
            try
            {
                _logger.LogInformation("GetContractInfoByRentId {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _cnpj);

                var contracts = await _contractService.GetContractInfoByRentId(rentId);
                return Ok(contracts);
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
        public async Task<IActionResult> UpdateContract([FromBody] UpdateContractRequest contractRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateContract {@dateTime} {@contractRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(contractRequest), id, _email, _cnpj);

                var validatedcontract = _updateContractValidator.Validate(contractRequest);

                if (!validatedcontract.IsValid)
                {
                    var contractValidationErros = new List<string>();
                    validatedcontract.Errors.ForEach(error => contractValidationErros.Add(error.ErrorMessage));
                    return BadRequest(contractValidationErros);
                }

                await _contractService.UpdateContract(contractRequest, id);

                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _contractService.DeleteById(id);
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
