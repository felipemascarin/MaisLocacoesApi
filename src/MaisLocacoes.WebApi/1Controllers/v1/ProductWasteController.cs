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
    [Route("api/v1/productwaste")]
    [ApiController]
    public class ProductWasteController : Controller
    {
        private readonly IProductWasteService _productWasteService;
        private readonly IValidator<CreateProductWasteRequest> _createProductWasteValidator;
        private readonly IValidator<UpdateProductWasteRequest> _updateProductWasteValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan _timeZone;
        private readonly string _email;
        private readonly string _schema;

        public ProductWasteController(IProductWasteService productWasteService,
            IValidator<CreateProductWasteRequest> createProductWasteValidator,
            IValidator<UpdateProductWasteRequest> updateProductWasteValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productWasteService = productWasteService;
            _createProductWasteValidator = createProductWasteValidator;
            _updateProductWasteValidator = updateProductWasteValidator;
            _logger = loggerFactory.CreateLogger<ProductWasteController>();
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _schema = JwtManager.GetSchemaByToken(_httpContextAccessor);
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductWaste([FromBody] CreateProductWasteRequest productWasteRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductWaste {@dateTime} {@productWasteRequest} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(productWasteRequest), _email, _schema);

                var validatedProductWaste = _createProductWasteValidator.Validate(productWasteRequest);

                if (!validatedProductWaste.IsValid)
                {
                    var productWasteValidationErros = new List<string>();
                    validatedProductWaste.Errors.ForEach(error => productWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productWasteValidationErros);
                }

                var productWasteCreated = await _productWasteService.CreateProductWaste(productWasteRequest);

                return CreatedAtAction(nameof(GetById), new { id = productWasteCreated.Id }, productWasteCreated);
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

                var productWaste = await _productWasteService.GetProductWasteById(id);
                return Ok(productWaste);
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

                var productWaste = await _productWasteService.GetAllProductWastesByProductId(productId);
                return Ok(productWaste);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("Log Warning: {@Message}", ex.Message);
                return StatusCode((int)ex.StatusCode, new GenericException(ex.Message));
            }
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpGet("items/{items}/page/{page}")]
        public async Task<IActionResult> GetProductWastesByPage(int items, int page, [FromQuery(Name = "query")] string query)
        {
            try
            {
                _logger.LogInformation("GetProductWastesByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, items, page, query, _email, _schema);

                var productWastesList = await _productWasteService.GetProductWastesByPage(items, page, query);
                return Ok(productWastesList);
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
        public async Task<IActionResult> UpdateProductWaste([FromBody] UpdateProductWasteRequest productWasteRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateProductWaste {@dateTime} {@ProductWasteRequest} id:{@id} User:{@email} Cnpj:{@cnpj}", System.DateTime.UtcNow + _timeZone, JsonConvert.SerializeObject(productWasteRequest), id, _email, _schema);

                var validatedProductWaste = _updateProductWasteValidator.Validate(productWasteRequest);

                if (!validatedProductWaste.IsValid)
                {
                    var productWasteValidationErros = new List<string>();
                    validatedProductWaste.Errors.ForEach(error => productWasteValidationErros.Add(error.ErrorMessage));
                    return BadRequest(productWasteValidationErros);
                }

                if (await _productWasteService.UpdateProductWaste(productWasteRequest, id)) return Ok();
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

                if (await _productWasteService.DeleteById(id)) return Ok();
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