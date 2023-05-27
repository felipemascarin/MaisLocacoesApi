using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
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
    [Route("api/v1/producttype")]
    [ApiController]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IValidator<ProductTypeRequest> _productTypeValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTypeController(IProductTypeService productTypeService,
            IValidator<ProductTypeRequest> productTypeValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productTypeService = productTypeService;
            _productTypeValidator = productTypeValidator;
            _logger = loggerFactory.CreateLogger<ProductTypeController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] ProductTypeRequest productTypeRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductType {@dateTime} {@productTypeRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTypeRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductType = _productTypeValidator.Validate(productTypeRequest);

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _productTypeService.DeleteById(id)) return Ok();
                else return StatusCode(500, new GenericException("Não foi possível deletar a fatura"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }
    }
}