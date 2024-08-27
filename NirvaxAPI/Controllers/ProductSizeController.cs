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
        public async Task<ActionResult<IEnumerable<ProductSizeListDTO>>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
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
                    return NoContent();
                }
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
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
                    return NoContent();

                }
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }




        [HttpGet("{productSizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetProductSizeByIdAsync(string productSizeId)
        {
            try { 
          
                    var productSize = await _repo.GetProductSizeByIdAsync(productSizeId);
                if (productSize != null) { 
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
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ViewQuantityStatisticsAsync(int ownerId)
        {
            var total = await _repo.ViewQuantityStatisticsAsync(ownerId);  

            return StatusCode(200, new
            {
                totalQuantityProductSize = total,          
            });
        }

   

    }
}
