using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using System.Net;

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
                return await Task.FromResult(Ok(billRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }


        //Endpoint para Update Status
    }
}