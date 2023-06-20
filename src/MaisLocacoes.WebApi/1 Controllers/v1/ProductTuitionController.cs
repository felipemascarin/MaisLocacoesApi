using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using Service.v1.Services;

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

                var productTuitionCreated = await _productTuitionService.CreateProductTuition(productTuitionRequest);

                return CreatedAtAction(nameof(GetById), new { id = productTuitionCreated.Id }, productTuitionCreated);
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

                var _productTuition = await _productTuitionService.GetById(id);
                return Ok(_productTuition);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("rent/{rentId}")]
        public async Task<IActionResult> GetAllByRentId(int rentId)
        {
            try
            {
                _logger.LogInformation("GetAllByRentId {@dateTime} rentId:{@rentId} User:{@email}", System.DateTime.Now, rentId, JwtManager.GetEmailByToken(_httpContextAccessor));

                var productTuition = await _productTuitionService.GetAllByRentId(rentId);
                return Ok(productTuition);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetAllByProductId(int productId)
        {
            try
            {
                _logger.LogInformation("GetAllByProductId {@dateTime} productId:{@productId} User:{@email}", System.DateTime.Now, productId, JwtManager.GetEmailByToken(_httpContextAccessor));

                var productTuitions = await _productTuitionService.GetAllByProductId(productId);
                return Ok(productTuitions);
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
        public async Task<IActionResult> UpdateProductTuition([FromBody] ProductTuitionRequest productTuitionRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateproductTuition {@dateTime} {@productTuitionRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productTuitionRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductTuition = _productTuitionValidator.Validate(productTuitionRequest);

                if (!validatedProductTuition.IsValid)
                {
                    var productTuitionValidationErros = new List<string>();
                    validatedProductTuition.Errors.ForEach(error => productTuitionValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productTuitionValidationErros);
                }

                if (await _productTuitionService.UpdateProductTuition(productTuitionRequest, id)) return Ok();
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
        [HttpPut("id/{id}/productcode/{productcode}")]
        public async Task<IActionResult> UpdateProductCode(string productCode, int id)
        {
            try
            {
                _logger.LogInformation("UpdateCode {@dateTime} productcode:{@productcode} id:{@id} User:{@email}", System.DateTime.Now, productCode, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (await _productTuitionService.UpdateProductCode(productCode, id)) return Ok();
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
        [HttpPut("id/{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string status, int id)
        {
            try
            {
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email}", System.DateTime.Now, status, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                if (!ProductTuitionStatus.ProductTuitionStatusEnum.Contains(status.ToLower()))
                    return BadRequest("Insira um status válido");

                if (await _productTuitionService.UpdateStatus(status, id)) return Ok();
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

                if (await _productTuitionService.DeleteById(id)) return Ok();
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