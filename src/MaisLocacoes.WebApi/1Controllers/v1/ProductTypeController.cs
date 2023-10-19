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
    [Route("api/v1/producttype")]
    [ApiController]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IValidator<CreateProductTypeRequest> _createProductTypeValidator;
        private readonly IValidator<UpdateProductTypeRequest> _updateProductTypeValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

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
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequest productTypeRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductType {@dateTime} {@productTypeRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTypeRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

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
                _logger.LogInformation("GetAll {@dateTime} User:{@email}", System.DateTime.Now, JwtManager.GetEmailByToken(_httpContextAccessor));

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
                _logger.LogInformation("UpdateProductType {@dateTime} {@productTypeRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTypeRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductType = _updateProductTypeValidator.Validate(productTypeRequest);

                if (!validatedProductType.IsValid)
                {
                    var productTypeValidationErros = new List<string>();
                    validatedProductType.Errors.ForEach(error => productTypeValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTypeValidationErros);
                }

                if (await _productTypeService.UpdateProductType(productTypeRequest, id)) return Ok();
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

                if (await _productTypeService.DeleteById(id)) return Ok();
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