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
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/bill")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        private readonly IValidator<CreateBillRequest> _createBillValidator;
        private readonly IValidator<UpdateBillRequest> _updateBillValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public BillController(IBillService billService,
            IValidator<CreateBillRequest> createBillValidator,
            IValidator<UpdateBillRequest> updateBillValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _billService = billService;
            _createBillValidator = createBillValidator;
            _updateBillValidator = updateBillValidator;
            _logger = loggerFactory.CreateLogger<BillController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillRequest billRequest)
        {
            try
            {
                _logger.LogInformation("CreateBill {@dateTime} {@billRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(billRequest), _email, _schema);

                var validatedBill = _createBillValidator.Validate(billRequest);

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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

                var bill = await _billService.GetBillById(id);
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
        [HttpGet("debts")]
        public async Task<IActionResult> GetAllDebts()
        {
            try
            {
                _logger.LogInformation("GetAllDebts {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _schema);

                var bill = await _billService.GetAllBillsDebts();
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
                _logger.LogInformation("GetForTaxInvoice {@dateTime} billId:{@billId} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), billId, _email, _schema);

                var billsList = await _billService.GetBillForTaxInvoice(billId);
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
                _logger.LogInformation("GetByRentTuitionId {@dateTime} rentId:{@rentId} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), rentId, _email, _schema);

                var billsList = await _billService.GetBillByRentId(rentId);
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
                _logger.LogInformation("GetDuedBills {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _schema);

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
        public async Task<IActionResult> UpdateBill([FromBody] UpdateBillRequest billRequest, int id)
        {
            try
            {
                _logger.LogInformation("Updatebill {@dateTime} {@billRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(billRequest), id, _email, _schema);

                var validatedbill = _updateBillValidator.Validate(billRequest);

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
        public async Task<IActionResult> UpdateStatus(string status, [FromQuery(Name = "paymentMode")] string paymentMode, [FromQuery(Name = "payDate")] DateTime? payDate, [FromQuery(Name = "nfIdFireBase")] int? nfIdFireBase, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} paymentMode:{@paymentMode} payDate:{@payDate} nfIdFireBase:{@nfIdFireBase} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), status, paymentMode, payDate, nfIdFireBase, id, _email, _schema);

                if (!BillStatus.BillStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _billService.UpdateStatus(status, paymentMode, payDate, nfIdFireBase, id)) return Ok();
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

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