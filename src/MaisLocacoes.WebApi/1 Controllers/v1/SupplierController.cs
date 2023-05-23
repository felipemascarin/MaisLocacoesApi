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
    [Route("api/v1/supplier")]
    [ApiController]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IValidator<SupplierRequest> _supplierValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierController(ISupplierService supplierService,
            IValidator<SupplierRequest> supplierValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _supplierService = supplierService;
            _supplierValidator = supplierValidator;
            _logger = loggerFactory.CreateLogger<SupplierController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierRequest supplierRequest)
        {
            try
            {
                _logger.LogInformation("CreateSupplier {@dateTime} {@supplierRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(supplierRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedSupplier = _supplierValidator.Validate(supplierRequest);

                if (!validatedSupplier.IsValid)
                {
                    var supplierValidationErros = new List<string>();
                    validatedSupplier.Errors.ForEach(error => supplierValidationErros.Add(error.ErrorMessage));
                    return BadRequest(supplierValidationErros);
                }
                return await Task.FromResult(Ok(supplierRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}