using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IService;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _service;

        public ProductController(IProductRepository productRepository, IMapper mapper, IImageService service, IImageRepository imageRepository, INotificationRepository notificationRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _service = service;
            _imageRepository = imageRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true || product.Isban == true)
                {
                    return NotFound(new { message = "Product is not found." });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(int ownerId)
        {
            try 
            { 
                var products = await _productRepository.GetByOwnerAsync(ownerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            { 
                var products = await _productRepository.GetByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            try
            { 
                var products = await _productRepository.GetByBrandAsync(brandId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string productName, [FromQuery] float? minPrice, [FromQuery] float? maxPrice, [FromQuery] int? categoryId, [FromQuery] int? brandId, [FromQuery] int? ownerId)
        {
            try
            { 
                var products = await _productRepository.SearchAsync(productName, minPrice, maxPrice, categoryId, brandId, ownerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var product = _mapper.Map<Product>(productDto);
                var check = await _productRepository.CheckProductAsync(product);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The product name has been duplicated." });
                }

                var result1 = await _productRepository.CreateAsync(product);
                if (!result1)
                {
                    StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the product." });
                }

                // Handle image upload
                if (productDto.ImageFiles != null && productDto.ImageFiles.Count > 0)
                {
                    foreach (var formFile in productDto.ImageFiles)
                    {
                        if (formFile.Length > 0)
                        {
                            var imagePath = _service.SaveImage(formFile, "products");

                            // Save image information to database
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = imagePath,
                                ProductId = product.ProductId
                            };
                            var result2 = await _imageRepository.AddImagesAsync(image);
                            if (!result2)
                            {
                                StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to add the image." });
                            }
                        }
                    }    
                }
                return Ok(new { message = "Product is created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDTO productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return NotFound(new { message = "Product is not found." });
                }
                
                _mapper.Map(productDto, product);
                var check = await _productRepository.CheckProductAsync(product);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The product name has been duplicated." });
                }

                var result1 = await _productRepository.UpdateAsync(product);
                if (!result1)
                {
                    StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update the product." });
                }

                IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetByProductAsync(id);
                foreach(BusinessObject.Models.Image img in images)
                {
                    // Xóa ảnh cũ trước khi cập nhật ảnh mới
                    if (!string.IsNullOrEmpty(img.LinkImage))
                    {
                        _service.DeleteImage(img.LinkImage);
                    }
                    var result2 = await _imageRepository.DeleteImagesAsync(img);
                    if (!result2)
                    {
                        StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to delete the image." });
                    }
                }
                // Handle image upload
                if (productDto.ImageFiles != null && productDto.ImageFiles.Count > 0)
                {
                    foreach (var formFile in productDto.ImageFiles)
                    {
                        if (formFile.Length > 0)
                        {
                            var imagePath = _service.SaveImage(formFile, "products");
                            // Save image information to database
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = imagePath,
                                ProductId = id
                            };
                            var result3 = await _imageRepository.AddImagesAsync(image);
                            if (!result3)
                            {
                                StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update the image." });
                            }
                        }
                    }
                }
                return Ok(new { message = "Product is updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            { 
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return NotFound(new { message = "Product not found." });
                }
                var result = await _productRepository.DeleteAsync(product);
                if (result)
                {
                    return Ok(new { message = "Product is deleted successfully." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("ban/{id}")]
        public async Task<IActionResult> BanProduct(int id)
        {
            try
            { 
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true || product.Isban == true)
                {
                    return NotFound(new { message = "Product not found." });
                }
                var result = await _productRepository.BanProductAsync(product);
                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = product.OwnerId, // Assuming Product model has OwnerId field
                    Content = $"Your product '{product.Name}' has been banned.",
                    IsRead = false,
                    Url = null,
                    CreateDate = DateTime.UtcNow
                };

                var notificationResult = await _notificationRepository.AddNotificationAsync(notification);
                if (!notificationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the notification." });
                }
                if (result)
                {
                    return Ok(new { message = "Product is banned successfully." });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{productId}/rate")]
        public async Task<IActionResult> RateProduct(int productId, [FromBody] int rating)
        {
            try
            {
                if (rating < 1 || rating > 5)
                {
                    return BadRequest(new { message = "Rating must be between 1 and 5." });
                }

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.Isdelete == true || product.Isban == true)
                {
                    return NotFound(new { message = "Product is not found." });
                }

                var result = await _productRepository.AddRatingAsync(product, rating);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to add the rating." });
                }

                return Ok(new { message = "Product is rated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("Top5")]
        public async Task<IActionResult> GetTop5SellingProducts()
        {
            try
            {
                var products = await _productRepository.GetTopSellingProductsAsync(5);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("Top10")]
        public async Task<IActionResult> GetTop10SellingProducts()
        {
            try
            {
                var products = await _productRepository.GetTopSellingProductsAsync(10);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
    }

}
