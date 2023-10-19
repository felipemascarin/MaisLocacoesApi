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
        private readonly IValidator<CreateProductTuitionValueRequest> _createProductTuitionValueValidator;
        private readonly IValidator<UpdateProductTuitionValueRequest> _updateProductTuitionValueValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionValueController(IProductTuitionValueService productTuitionValueService,
         IValidator<CreateProductTuitionValueRequest> createProductTuitionValueValidator,
         IValidator<UpdateProductTuitionValueRequest> updateProductTuitionValueValidator,
         ILoggerFactory loggerFactory,
         IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionValueService = productTuitionValueService;
            _createProductTuitionValueValidator = createProductTuitionValueValidator;
            _updateProductTuitionValueValidator = updateProductTuitionValueValidator;
            _logger = loggerFactory.CreateLogger<CreateProductTuitionValueRequest>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductTuitionValue([FromBody] CreateProductTuitionValueRequest productTuitionValueRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductTuitionValue {@dateTime} {@productTuitionValueRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionValueRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuitionValue = _createProductTuitionValueValidator.Validate(productTuitionValueRequest);

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

                var _productTuitionValue = await _productTuitionValueService.GetProductTuitionValueById(id);
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

                var productTuitionValue = await _productTuitionValueService.GetAllProductTuitionValueByProductTypeId(productTypeId);
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
        public async Task<IActionResult> UpdateProductTuitionValue([FromBody] UpdateProductTuitionValueRequest productTuitionValueRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateproductTuitionValue {@dateTime} {@productTuitionValueRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionValueRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuitionValue = _updateProductTuitionValueValidator.Validate(productTuitionValueRequest);

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