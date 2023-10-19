using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Exceptions;
using MaisLocacoes.WebApi.Utils.Annotations;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.v1.IServices;
using System.Net;

namespace MaisLocacoes.WebApi.Controllers.v1
{
    [Route("api/v1/producttuition")]
    [ApiController]
    public class ProductTuitionController : Controller
    {
        private readonly IProductTuitionService _productTuitionService;
        private readonly IValidator<CreateProductTuitionRequest> _createProductTuitionValidator;
        private readonly IValidator<UpdateProductTuitionRequest> _updateProductTuitionValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public ProductTuitionController(IProductTuitionService productTuitionService,
        IValidator<CreateProductTuitionRequest> createProductTuitionValidator,
        IValidator<UpdateProductTuitionRequest> updateProductTuitionValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionService = productTuitionService;
            _createProductTuitionValidator = createProductTuitionValidator;
            _updateProductTuitionValidator = updateProductTuitionValidator;
            _logger = loggerFactory.CreateLogger<CreateProductTuitionRequest>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductTuition([FromBody] CreateProductTuitionRequest productTuitionRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductTuition {@dateTime} {@productTuitionRequest} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(productTuitionRequest), _email, _schema);

                var validatedProductTuition = _createProductTuitionValidator.Validate(productTuitionRequest);

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
        [HttpPost("withdraw/{id}")]
        public async Task<IActionResult> WithdrawProduct(int id)
        {
            try
            {
                _logger.LogInformation("WithdrawProduct {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

                if (await _productTuitionService.WithdrawProduct(id)) return Ok();
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
        [HttpPost("cancelwithdraw/{id}")]
        public async Task<IActionResult> CancelWithdrawProduct(int id)
        {
            try
            {
                _logger.LogInformation("CancelWithdrawProduct {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

                if (await _productTuitionService.CancelWithdrawProduct(id)) return Ok();
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
        [HttpPost("renew/{id}")]
        public async Task<IActionResult> RenewProduct([FromBody] RenewProductTuitionRequest renewRequest, int id)
        {
            try
            {
                _logger.LogInformation("RenewProduct {@dateTime} request:{@request} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(renewRequest), id, _email, _schema);

                if (await _productTuitionService.RenewProductTuition(id, renewRequest)) return Ok();
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

                var _productTuition = await _productTuitionService.GetProductTuitionById(id);
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
                _logger.LogInformation("GetAllByRentId {@dateTime} rentId:{@rentId} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, rentId, _email, _schema);

                var productTuition = await _productTuitionService.GetAllProductTuitionByRentId(rentId);
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
                _logger.LogInformation("GetAllByProductId {@dateTime} productId:{@productId} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, productId, _email, _schema);

                var productTuitions = await _productTuitionService.GetAllProductTuitionByProductId(productId);
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
        [HttpGet("toremove")]
        public async Task<IActionResult> GetAllToRemove()
        {
            try
            {
                _logger.LogInformation("GetAllToRemove {@dateTime} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, _email, _schema);

                var productTuitions = await _productTuitionService.GetAllProductTuitionToRemove();
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
        public async Task<IActionResult> UpdateProductTuition([FromBody] UpdateProductTuitionRequest productTuitionRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateproductTuition {@dateTime} {@productTuitionRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(productTuitionRequest), id, _email, _schema);

                var validatedProductTuition = _updateProductTuitionValidator.Validate(productTuitionRequest);

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
                _logger.LogInformation("UpdateCode {@dateTime} productcode:{@productcode} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, productCode, id, _email, _schema);

                if (productCode == null)
                    throw new HttpRequestException("Código do produto é obrigatório", null, HttpStatusCode.BadRequest);

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
                _logger.LogInformation("UpdateStatus {@dateTime} status:{@status} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, status, id, _email, _schema);

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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, id, _email, _schema);

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