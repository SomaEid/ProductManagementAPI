using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSolution.Application.DTOs;
using ProductManagementSolution.Application.Interfaces;

namespace ProductManagementSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAll([FromQuery] PaginationParams @params)
            => Ok(await _productService.GetAllProductsAsync(@params));


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            _logger.LogInformation("Creating a new product with name: {ProductName} by user: {User}",
            dto.ProductName, dto.CreatedBy);
            try
            {
                var result = await _productService.CreateProductAsync(dto);

                if (result == null)
                {
                    _logger.LogWarning("Failed to create product: {ProductName}. Service returned null.", dto.ProductName);
                    return BadRequest("Could not create product.");
                }

                _logger.LogInformation("Product {ProductName} created successfully with ID: {ProductId}",
                    result.ProductName, result.Id);

                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating product: {ProductName}", dto.ProductName);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ProductDto dto)
        {
            var result = await _productService.updateProduct(dto);
            if (result == null)
                return NotFound(new { status = false, message = $"No Product with this id {dto.Id} Founded" });

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Attempting to delete product with ID: {id}");

            var isDeleted = await _productService.DeleteProduct(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Product with ID: {id} was not found for deletion");
                return NotFound(new { status = false, message = $"Product with ID {id} not found." });
            }

            return NoContent();
        }

    }
}
