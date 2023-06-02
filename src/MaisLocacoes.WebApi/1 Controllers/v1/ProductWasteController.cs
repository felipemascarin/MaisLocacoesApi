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
    [Route("api/v1/productwaste")]
    [ApiController]
    public class ProductWasteController : Controller
    {
        private readonly IProductWasteService _productWasteService;
        private readonly IValidator<ProductWasteRequest> _productWasteValidator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductWasteController(IProductWasteService productWasteService,
            IValidator<ProductWasteRequest> productWasteValidator,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _productWasteService = productWasteService;
            _productWasteValidator = productWasteValidator;
            _logger = loggerFactory.CreateLogger<ProductWasteController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [TokenValidationDataBase]
        [HttpPost]
        public async Task<IActionResult> CreateProductWaste([FromBody] ProductWasteRequest productWasteRequest)
        {
            try
            {
                _logger.LogInformation("CreateProductWaste {@dateTime} {@productWasteRequest} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productWasteRequest), JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductWaste = _productWasteValidator.Validate(productWasteRequest);

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
                _logger.LogInformation("GetById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var productWaste = await _productWasteService.GetById(id);
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
        [HttpGet("allproducts/{id}")]
        public async Task<IActionResult> GetAllById(int id)
        {
            try
            {
                _logger.LogInformation("GetAllById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var productWaste = await _productWasteService.GetAllById(id);
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
                _logger.LogInformation("GetProductWastesByPage {@dateTime} items:{@items} pages:{@page} query:{@query} User:{@email}", System.DateTime.Now, items, page, query, JwtManager.GetEmailByToken(_httpContextAccessor));

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
        public async Task<IActionResult> UpdateProductWaste([FromBody] ProductWasteRequest productWasteRequest, int id)
        {
            try
            {
                _logger.LogInformation("UpdateProductWaste {@dateTime} {@ProductWasteRequest} id:{@id} User:{@email}", System.DateTime.Now, JsonConvert.SerializeObject(productWasteRequest), id, JwtManager.GetEmailByToken(_httpContextAccessor));

                var validatedProductWaste = _productWasteValidator.Validate(productWasteRequest);

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
                _logger.LogInformation("DeleteById {@dateTime} id:{@id} User:{@email}", System.DateTime.Now, id, JwtManager.GetEmailByToken(_httpContextAccessor));

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