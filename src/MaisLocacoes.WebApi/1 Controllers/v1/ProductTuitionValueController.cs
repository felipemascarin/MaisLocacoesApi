using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;

namespace MaisLocacoes.WebApi._1_Controllers.v1
{
    [Route("api/v1/productTuitionvalue")]
    [ApiController]
    public class ProductTuitionValueController : Controller
    {
        private readonly IProductTuitionValueService _productTuitionValueService;
        private readonly IValidator<ProductTuitionValueRequest> _productTuitionValueValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionValueController(IProductTuitionValueService productTuitionValueService,
         IValidator<ProductTuitionValueRequest> productTuitionValueValidator,
         ILoggerFactory loggerFactory,
         IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionValueService = productTuitionValueService;
            _productTuitionValueValidator = productTuitionValueValidator;
            _logger = loggerFactory.CreateLogger<ProductTuitionValueRequest>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductTuitionValue([FromBody] ProductTuitionValueRequest productTuitionValueRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductTuitionValue {@dateTime} {@productTuitionValueRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionValueRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuitionValue = _productTuitionValueValidator.Validate(productTuitionValueRequest);

                if (!validatedProductTuitionValue.IsValid)
                {
                    var productTuitionValueValidationErros = new List<string>();
                    validatedProductTuitionValue.Errors.ForEach(error => productTuitionValueValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTuitionValueValidationErros);
                }

                var productTuitionValueCreated = await _productTuitionValueService.CreateProductTuitionValue(productTuitionValueRequest);

                return CreatedAtAction(nameof(GetById), new { id = productTuitionValueCreated.Id }, productTuitionValueCreated);
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

                var _productTuitionValue = await _productTuitionValueService.GetById(id);
                return Ok(_productTuitionValue);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("producttypeid/{productTypeId}")]
        public async Task<IActionResult> GetAllByProductTypeId(int productTypeId)
        {
            try
            {
                _logger.LogInformation("GetAllByProductTypeId {@dateTime} rentId:{@rentId} User:{@email}", System.DateTime.Now, productTypeId, JwtManager.GetEmailByToken(_httpContextAccessor));

                var productTuitionValue = await _productTuitionValueService.GetAllByProductTypeId(productTypeId);
                return Ok(productTuitionValue);
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
        public async Task<IActionResult> UpdateProductTuitionValue([FromBody] ProductTuitionValueRequest productTuitionValueRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateproductTuitionValue {@dateTime} {@productTuitionValueRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionValueRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuitionValue = _productTuitionValueValidator.Validate(productTuitionValueRequest);

                if (!validatedProductTuitionValue.IsValid)
                {
                    var productTuitionValueValidationErros = new List<string>();
                    validatedProductTuitionValue.Errors.ForEach(error => productTuitionValueValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTuitionValueValidationErros);
                }

                if (await _productTuitionValueService.UpdateProductTuitionValue(productTuitionValueRequest, id)) return Ok();
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

                if (await _productTuitionValueService.DeleteById(id)) return Ok();
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