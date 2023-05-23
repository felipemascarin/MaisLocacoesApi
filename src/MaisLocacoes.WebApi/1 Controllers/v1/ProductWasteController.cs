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
    [Route("api/v1/productwaste")]
    [ApiController]
    public class ProductWasteController : Controller
    {
        private readonly IProductWasteService _productWasteService;
        private readonly IValidator<ProductWasteRequest> _productWasteValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductWasteController(IProductWasteService productWasteService,
            IValidator<ProductWasteRequest> productWasteValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productWasteService = productWasteService;
            _productWasteValidator = productWasteValidator;
            _logger = loggerFactory.CreateLogger<ProductWasteController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductWaste([FromBody] ProductWasteRequest productWasteRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductWaste {@dateTime} {@productWasteRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productWasteRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductWaste = _productWasteValidator.Validate(productWasteRequest);

                if (!validatedProductWaste.IsValid)
                {
                    var productWasteValidationErros = new List<string>();
                    validatedProductWaste.Errors.ForEach(error => productWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productWasteValidationErros);
                }
                return await Task.FromResult(Ok(productWasteRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}