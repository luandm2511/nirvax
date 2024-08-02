using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProductSizeController : ControllerBase
    {
        private readonly IProductSizeRepository  _repo;
        private readonly string ok = "successfully ";
        private readonly string notFound = "Not found ";
        private readonly string badRequest = "Failed! ";

        public ProductSizeController(IProductSizeRepository repo)
        {
             _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            try { 
            var list = await _repo.GetAllProductSizesAsync(searchQuery, page, pageSize, ownerId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list productSize " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any productSize"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSizeByProductIdAsync(int productId)
        {
            try { 
            var list = await _repo.GetProductSizeByProductIdAsync(productId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list productSize " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any productSize"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }




        [HttpGet("{productSizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetProductSizeByIdAsync(string productSizeId)
        {
            try { 
            var checkProductSize = await _repo.CheckProductSizeByIdAsync(productSizeId);
                if (checkProductSize == true)
                {
                    var productSize = await _repo.GetProductSizeByIdAsync(productSizeId);
                    return StatusCode(200, new
                    {
                        Message = "Get productSize by id " + ok,
                        Data = productSize
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any productSize"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


        [HttpPatch("{productSizeId}")]
        public async Task<ActionResult> DeleteProductSizeAsync(string productSizeId)
        {
            try { 
            var productSize1 = await _repo.DeleteProductSizeAsync(productSizeId);
                if (productSize1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Delete productSize " + ok,

                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

    }
}
