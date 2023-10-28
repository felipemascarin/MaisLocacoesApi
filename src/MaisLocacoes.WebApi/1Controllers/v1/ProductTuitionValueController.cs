using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using TimeZoneConverter;

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
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _schema;

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
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductTuitionValue([FromBody] CreateProductTuitionValueRequest productTuitionValueRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductTuitionValue {@dateTime} {@productTuitionValueRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(productTuitionValueRequest), _email, _schema);

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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _schema);

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
                _logger.LogInformation("GetAllByProductTypeId {@dateTime} rentId:{@rentId} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), productTypeId, _email, _schema);

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
                _logger.LogInformation("UpdateproductTuitionValue {@dateTime} {@productTuitionValueRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(productTuitionValueRequest), id, _email, _schema);

                var validatedProductTuitionValue = _updateProductTuitionValueValidator.Validate(productTuitionValueRequest);

                if (!validatedProductTuitionValue.IsValid)
                {
                    var productTuitionValueValidationErros = new List<string>();
                    validatedProductTuitionValue.Errors.ForEach(error => productTuitionValueValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTuitionValueValidationErros);
                }

                await _productTuitionValueService.UpdateProductTuitionValue(productTuitionValueRequest, id);
                
                return Ok();
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

                await _productTuitionValueService.DeleteById(id);
                
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}