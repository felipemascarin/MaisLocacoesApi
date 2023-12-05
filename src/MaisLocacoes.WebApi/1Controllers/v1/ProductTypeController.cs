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

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/producttype")]
    [ApiController]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IValidator<CreateProductTypeRequest> _createProductTypeValidator;
        private readonly IValidator<UpdateProductTypeRequest> _updateProductTypeValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public ProductTypeController(IProductTypeService productTypeService,
            IValidator<CreateProductTypeRequest> createProductTypeValidator,
            IValidator<UpdateProductTypeRequest> updateProductTypeValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productTypeService = productTypeService;
            _createProductTypeValidator = createProductTypeValidator;
            _updateProductTypeValidator = updateProductTypeValidator;
            _logger = loggerFactory.CreateLogger<ProductTypeController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequest productTypeRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductType {@dateTime} {@productTypeRequest} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(productTypeRequest), _email, _cnpj);

                var validatedProductType = _createProductTypeValidator.Validate(productTypeRequest);

                if (!validatedProductType.IsValid)
                {
                    var productTypeValidationErros = new List<string>();
                    validatedProductType.Errors.ForEach(error => productTypeValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTypeValidationErros);
                }
                var productTypeCreated = await _productTypeService.CreateProductType(productTypeRequest);

                return Ok(productTypeCreated);
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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                var productType = await _productTypeService.GetProductTypeById(id);
                return Ok(productType);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll {@dateTime} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), _email, _cnpj);

                var productTypes = await _productTypeService.GetAllProductTypes();
                return Ok(productTypes);
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
        public async Task<IActionResult> UpdateProductType([FromBody] UpdateProductTypeRequest productTypeRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateProductType {@dateTime} {@productTypeRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), JsonConvert.SerializeObject(productTypeRequest), id, _email, _cnpj);

                var validatedProductType = _updateProductTypeValidator.Validate(productTypeRequest);

                if (!validatedProductType.IsValid)
                {
                    var productTypeValidationErros = new List<string>();
                    validatedProductType.Errors.ForEach(error => productTypeValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTypeValidationErros);
                }

                await _productTypeService.UpdateProductType(productTypeRequest, id);
                
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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone), id, _email, _cnpj);

                await _productTypeService.DeleteById(id);
                
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