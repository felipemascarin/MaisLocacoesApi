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
using Service.v1.Services;

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
        [HttpGet("taxinvoice/{billId}")]
        public async Task<IActionResult> GetForTaxInvoice(int billId)
        {
            try
            {
                _logger.LogInformation("GetForTaxInvoice {@dateTime} billId:{@billId} User:{@email}", System.DateTime.Now, billId, JwtManager.GetEmailByToken(_httpContextAccessor));

                var billsList = await _billService.GetForTaxInvoice(billId);
                return Ok(billsList);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("rentid/{rentId}")]
        public async Task<IActionResult> GetByRentId(int rentId)
        {
            try
            {
                _logger.LogInformation("GetByRentTuitionId {@dateTime} rentId:{@rentId} User:{@email}", System.DateTime.Now, rentId, JwtManager.GetEmailByToken(_httpContextAccessor));

                var billsList = await _billService.GetByRentId(rentId);
                return Ok(billsList);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("duedbills")]
        public async Task<IActionResult> GetDuedBills()
        {
            try
            {
                _logger.LogInformation("GetDuedBills {@dateTime} User:{@email}", System.DateTime.Now, JwtManager.GetEmailByToken(_httpContextAccessor));

                var billsList = await _billService.GetDuedBills();
                return Ok(billsList);
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
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, [FromQuery(Name = "paymentMode")] string paymentMode, [FromQuery(Name = "payDate")] DateTime? payDate, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} paymentMode:{@paymentMode} id:{@id} User:{@email}", System.DateTime.Now, status, paymentMode, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!BillStatus.BillStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _billService.UpdateStatus(status, paymentMode, payDate, id)) return Ok();
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

                if (await _billService.DeleteById(id)) return Ok();
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