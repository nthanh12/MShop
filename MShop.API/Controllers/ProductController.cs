using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MShop.Application.Constants;
using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Application.Interfaces;
using MShop.Application.Responses;
using MShop.Application.SharedFunct;

namespace MShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetProducts(string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.ID, bool isDescending = false, int pageNumber = 1, int pageSize = 10)
        {
            var response = new ApiResponse<PagedResult<ProductDto>>();
            try
            {
                _logger.LogInformation("Getting all products.");

                var products = await _productService.GetProductsAsync(searchValue, searchType, orderBy, isDescending, pageNumber, pageSize);

                if (products.Items == null || !products.Items.Any())
                {
                    _logger.LogWarning("No product found");
                    response.ErrCode = EnumErrCode.DoesNotExist;
                    response.ErrDescription = ApiMessage.ProductNotMatch;
                    response.ErrDetail = "There are no product available.";

                    return NotFound(response);
                }

                response.Data = products;
                response.ErrCode = EnumErrCode.Success;
                response.ErrDescription = ApiMessage.ProductRetrievedSuccessfully;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                var errorResponse = Utils.ActionCatch<IEnumerable<ProductDto>>(EnumErrCode.Error, ApiMessage.ErrorMsg, ex.Message);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(int id)
        {
            var response = new ApiResponse<ProductDto>();
            try
            {
                _logger.LogInformation("Getting product with ID: {id}", id); //ID truy vấn
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {id} not found.", id); //Cảnh báo 
                    response.ErrCode = EnumErrCode.DoesNotExist;
                    response.ErrDescription = ApiMessage.ProductNotFound;
                    response.ErrDetail = "No product found with the specified ID";

                    return NotFound(response);
                }
                response.Data = product;
                response.ErrCode = EnumErrCode.Success;
                response.ErrDescription = "Product retrieved successfully";
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with ID: {id}", id); // Log lỗi khi xảy ra exception
                var errorResponse = Utils.ActionCatch<ProductDto>(EnumErrCode.Error, ApiMessage.ErrorMsg, ex.Message);
                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            await _productService.AddProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            var existProduct = await _productService.GetProductByIdAsync(id);
            if (existProduct == null) return NotFound();
            await _productService.UpdateProductAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existProduct = await _productService.GetProductByIdAsync(id);
            if (existProduct == null) return NotFound();
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
