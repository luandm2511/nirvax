﻿using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using WebAPI.Service;
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
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IAccessLogService _accessLogService;
        private readonly IMapper _mapper;

        public ProductController(IHubContext<NotificationHub> hubContext, IProductRepository productRepository, ITransactionRepository transactionRepository, IMapper mapper, IImageRepository imageRepository, INotificationRepository notificationRepository, IAccessLogService accessLogService)
        {
            _hubContext = hubContext;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
            _notificationRepository = notificationRepository;
            _accessLogService = accessLogService;
        }

        [HttpGet("dashboard-admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("home")]
        public async Task<IActionResult> GetAllInHome()
        {
            try
            {
                await _accessLogService.LogAccessAsync(HttpContext);
                var products = await _productRepository.GetProductsInHomeAsync();
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404,new { message = "The product has been not found." });
                }
                return Ok(product);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("home/{id}")]
        public async Task<IActionResult> GetProductDetailInHome(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404, new { message = "The product has been not found." });
                }
                if (product.Isban)
                {
                    return StatusCode(404, new { message = "The product has been banned." });
                }
                return Ok(product);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("home/owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(int ownerId)
        {
            try 
            { 
                var products = await _productRepository.GetByOwnerAsync(ownerId);
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("dashboard-owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerInDashboard(int ownerId)
        {
            try
            {
                var products = await _productRepository.GetByOwnerInDashboardAsync(ownerId);
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("home/category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            { 
                var products = await _productRepository.GetByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("home/brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            try
            { 
                var products = await _productRepository.GetByBrandAsync(brandId);
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }
        [HttpPost]
        //[Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> Create([FromForm] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            { 
                var product = _mapper.Map<Product>(productDto);
                var check = await _productRepository.CheckProductAsync(product);
                if (!check)
                {
                    return StatusCode(406, new { message = "The product name has been duplicated." });
                }

                await _productRepository.CreateAsync(product);

                foreach (var link in productDto.ImageLinks)
                {
                        // Save image information to database
                        var image = new BusinessObject.Models.Image
                        {
                            LinkImage = link,
                            ProductId = product.ProductId
                        };
                        await _imageRepository.AddImagesAsync(image);
                }
                await _transactionRepository.CommitTransactionAsync();
                return Ok(new { message = "Product is created successfully." });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404,new { message = "The product has been not found." });
                }
                
                _mapper.Map(productDto, product);
                var check = await _productRepository.CheckProductAsync(product);
                if (!check)
                {
                    return StatusCode(406, new { message = "The product name has been duplicated." });
                }

                await _productRepository.UpdateAsync(product);

                IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetByProductAsync(id);
                foreach(BusinessObject.Models.Image img in images)
                {
                    // Xóa ảnh cũ trước khi cập nhật ảnh mới
                    await _imageRepository.DeleteImagesAsync(img);
                }
                // Handle image upload
                foreach (var link in productDto.ImageLinks)
                {
                    // Save image information to database
                    var image = new BusinessObject.Models.Image
                    {
                        LinkImage = link,
                        ProductId = product.ProductId
                    };
                    await _imageRepository.AddImagesAsync(image);
                }
                await _transactionRepository.CommitTransactionAsync();
                return Ok(new { message = "Product is updated successfully." });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            { 
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Product not found." });
                }
                await _productRepository.DeleteAsync(product);
                return Ok(new { message = "Product is deleted successfully"});
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("ban/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanProduct(int id)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            { 
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Product not found." });
                }
                await _productRepository.BanProductAsync(product);
                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = product.OwnerId, // Assuming Product model has OwnerId field
                    Content = $"Your product '{product.Name}' has been banned.",
                    IsRead = false,
                    Url = "abcd",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"Owner-{product.OwnerId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok(new { message = "Product is banned successfully." });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("unban/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnbanProduct(int id)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Product not found." });
                }
                await _productRepository.UnbanProductAsync(product);
                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = product.OwnerId, // Assuming Product model has OwnerId field
                    Content = $"Your product '{product.Name}' has been unbanned.",
                    IsRead = false,
                    Url = "abcd",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"Owner-{product.OwnerId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok(new { message = "Product has been unbanned successfully." });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();    
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("rate/{productId}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> RateProduct(int productId, double rating, int orderId, string productSizeId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.Isdelete == true || product.Isban)
                {
                    return StatusCode(404,new { message = "The product has been deleted or banned. You can't rate and comment product" });
                }
                var orderDetail = await _productRepository.GetOrderDetailAsync(orderId, productSizeId);
                orderDetail.UserRate = rating;
                await _productRepository.AddRatingAsync(product, rating);

                return Ok(new { message = "Product is rated successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("top5/{ownerId}")]
        public async Task<IActionResult> GetTop5SellingProductsByOwner(int ownerId)
        {
            try
            {
                var products = await _productRepository.GetTopSellingProductsByOwnerAsync(ownerId);
                return Ok(products);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("top10")]
        public async Task<IActionResult> GetTop10SellingProducts()
        {
            try
            {
                var products = await _productRepository.GetTopSellingProductsAsync();
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("admin/searchs")]
        public async Task<IActionResult> SearchsProductsInAdmin(string? searchTerm)
        {
            try
            {
                var result = await _productRepository.SearchProductsInAdminAsync(searchTerm);
                return Ok(result);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }   
        }

        [HttpGet("owner/searchs/{ownerId}")]
        public async Task<IActionResult> SearchsProductsInOwner(string? searchTerm, int ownerId)
        {
            try
            {
                var result = await _productRepository.SearchProductsInOwnerAsync(searchTerm, ownerId);
                return Ok(result);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpGet("home/searchs")]
        public async Task<IActionResult> Searchs(string? searchTerm, double? minPrice, double? maxPrice, [FromQuery] List<int> categoryIds, [FromQuery] List<int> brandIds, [FromQuery] List<string> sizes)
        {
            try
            {
                var result = await _productRepository.SearchProductsAndOwnersAsync(searchTerm, minPrice, maxPrice, categoryIds, brandIds, sizes);
                return Ok(result);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }
    }

}
  