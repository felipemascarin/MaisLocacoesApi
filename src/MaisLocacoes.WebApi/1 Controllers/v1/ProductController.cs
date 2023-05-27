using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Validator;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using Service.v1.Services;

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

                var productCreated = await _productService.CreateProduct(productRequest);

                return Ok(productCreated);
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

                var product = await _productService.GetById(id);
                return Ok(product);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("typeId/{typeId}/code/{code}")]
        public async Task<IActionResult> GetByTypeCode(int typeId, string code)
        {
            try
            {
                _logger.LogInformation("GetByTypeCode {@dateTime} typeId:{@typeId} code:{@code} User:{@email}", System.DateTime.Now, typeId, code, JwtManager.GetEmailByToken(_httpContextAccessor));

                var product = await _productService.GetByTypeCode(typeId, code);
                return Ok(product);
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
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest productRequest, int id)
        {
            try
            {
                _logger.LogInformation("Updateproduct {@dateTime} {@productRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProduct = _productValidator.Validate(productRequest);

                if (!validatedProduct.IsValid)
                {
                    var productValidationErros = new List<string>();
                    validatedProduct.Errors.ForEach(error => productValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productValidationErros);
                }

                if (await _productService.UpdateProduct(productRequest, id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível alterar o produto"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}