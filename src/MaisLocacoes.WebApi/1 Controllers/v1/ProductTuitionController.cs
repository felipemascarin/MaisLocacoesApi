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
    [Route("api/v1/producttuition")]
    [ApiController]
    public class ProductTuitionController : Controller
    {
        private readonly IProductTuitionService _productTuitionService;
        private readonly IValidator<ProductTuitionRequest> _productTuitionValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionController(IProductTuitionService productTuitionService,
            IValidator<ProductTuitionRequest> productTuitionValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionService = productTuitionService;
            _productTuitionValidator = productTuitionValidator;
            _logger = loggerFactory.CreateLogger<ProductTuitionRequest>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductTuition([FromBody] ProductTuitionRequest productTuitionRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductTuition {@dateTime} {@productTuitionRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuition = _productTuitionValidator.Validate(productTuitionRequest);

                if (!validatedProductTuition.IsValid)
                {
                    var productTuitionValidationErros = new List<string>();
                    validatedProductTuition.Errors.ForEach(error => productTuitionValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTuitionValidationErros);
                }
                return await Task.FromResult(Ok(productTuitionRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}