using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/bill")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        private readonly IValidator<BillRequest> _billValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillController(IBillService billService,
            IValidator<BillRequest> billValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _billService = billService;
            _billValidator = billValidator;
            _logger = loggerFactory.CreateLogger<BillController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] BillRequest billRequest)
        {
            try
            {
                _logger.LogInformation("CreateBill {@dateTime} {@billRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(billRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedBill = _billValidator.Validate(billRequest);

                if (!validatedBill.IsValid)
                {
                    var billValidationErros = new List<string>();
                    validatedBill.Errors.ForEach(error => billValidationErros.Add(error.ErrorMessage));
                    return BadRequest(billValidationErros);
                }

                var billCreated = await _billService.CreateBill(billRequest);

                return CreatedAtAction(nameof(GetById), new { id = billCreated.Id }, billCreated);
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

                var bill = await _billService.GetById(id);
                return Ok(bill);
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
        public async Task<IActionResult> UpdateBill([FromBody] BillRequest billRequest, int id)
        {
            try
            {
                _logger.LogInformation("Updatebill {@dateTime} {@billRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(billRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedbill = _billValidator.Validate(billRequest);

                if (!validatedbill.IsValid)
                {
                    var billValidationErros = new List<string>();
                    validatedbill.Errors.ForEach(error => billValidationErros.Add(error.ErrorMessage));
                    return BadRequest(billValidationErros);
                }

                if (await _billService.UpdateBill(billRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar a fatura"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email}", System.DateTime.Now, status, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!BillStatus.BillStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _billService.UpdateStatus(status, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar a fatura"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        //[Authorize]
        //[TokenValidationDataBase]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteById(int id)
        //{
        //}
    }
}