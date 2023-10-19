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
    [Route("api/v1/companytuition")]
    [ApiController]
    public class CompanyTuitionController : Controller
    {
        private readonly ICompanyTuitionService _companyTuitionService;
        private readonly IValidator<CreateCompanyTuitionRequest> _createCompanyTuitionValidator;
        private readonly IValidator<UpdateCompanyTuitionRequest> _updateTuitionValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyTuitionController(ICompanyTuitionService companyTuitionService,
            IValidator<CreateCompanyTuitionRequest> createCompanyTuitionValidator,
            IValidator<UpdateCompanyTuitionRequest> updateTuitionValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _companyTuitionService = companyTuitionService;
            _createCompanyTuitionValidator = createCompanyTuitionValidator;
            _updateTuitionValidator = updateTuitionValidator;
            _logger = loggerFactory.CreateLogger<CompanyTuitionController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyTuition([FromBody] CreateCompanyTuitionRequest companyTuitionRequest)
        {
            try
            {
                _logger.LogInformation("CreateCompanyTuition {@dateTime} {@companyTuitionRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyTuitionRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompanyTuition = _createCompanyTuitionValidator.Validate(companyTuitionRequest);

                if (!validatedCompanyTuition.IsValid)
                {
                    var companyTuitionValidationErros = new List<string>();
                    validatedCompanyTuition.Errors.ForEach(error => companyTuitionValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyTuitionValidationErros);
                }
                
                var companyTuitionCreated = await _companyTuitionService.CreateCompanyTuition(companyTuitionRequest);

                return CreatedAtAction(nameof(GetById), new { id = companyTuitionCreated.Id }, companyTuitionCreated);
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

                var companyTuition = await _companyTuitionService.GetCompanyTuitionById(id);
                return Ok(companyTuition);
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
        public async Task<IActionResult> UpdateCompanyTuition([FromBody] UpdateCompanyTuitionRequest companyTuitionRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateCompanyTuition {@dateTime} {@companyTuitionRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(companyTuitionRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedCompanyTuition = _updateTuitionValidator.Validate(companyTuitionRequest);

                if (!validatedCompanyTuition.IsValid)
                {
                    var companyTuitionValidationErros = new List<string>();
                    validatedCompanyTuition.Errors.ForEach(error => companyTuitionValidationErros.Add(error.ErrorMessage));
                    return BadRequest(companyTuitionValidationErros);
                }

                if (await _companyTuitionService.UpdateCompanyTuition(companyTuitionRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar"));
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _companyTuitionService.DeleteById(id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível deletar"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}