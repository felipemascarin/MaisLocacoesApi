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
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IValidator<ProductRequest> _productValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService,
            IValidator<ProductRequest> productValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _productValidator = productValidator;
            _logger = loggerFactory.CreateLogger<ProductController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest productRequest)
        {
            try
            {
                _logger.LogInformation("CreateProduct {@dateTime} {@productRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProduct = _productValidator.Validate(productRequest);

                if (!validatedProduct.IsValid)
                {
                    var productValidationErros = new List<string>();
                    validatedProduct.Errors.ForEach(error => productValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productValidationErros);
                }
                return await Task.FromResult(Ok(productRequest));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}