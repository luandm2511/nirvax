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
        private readonly IConfiguration _config;
        private readonly IProductSizeRepository  _repo;
        private readonly string ok = "successfully ";
        private readonly string notFound = "Not found ";
        private readonly string badRequest = "Failed! ";

        public ProductSizeController(IConfiguration config, IProductSizeRepository repo)
        {
            _config = config;
             _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllProductSizesAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200,new
                {
                    Message = "Get list productSize " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {              
                Message = notFound + "any productSize"
            });
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSizeByProductIdAsync(int productId)
        {
            var list = await _repo.GetProductSizeByProductIdAsync(productId);
            if (list.Any())
            {
                return StatusCode(200,new
                {
                    Message = "Get list productSize " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any productSize"
            });
        }




        [HttpGet("{productSizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetProductSizeByIdAsync(string productSizeId)
        {
            var checkProductSize = await _repo.CheckProductSizeByIdAsync(productSizeId);
            if (checkProductSize == true)
            {
                var productSize = await _repo.GetProductSizeByIdAsync(productSizeId);
                return StatusCode(200,new
                {
                    Message = "Get productSize by id " + ok,
                    Data = productSize
                });
            }

            return StatusCode(404, new
            { 
                Message = notFound + "any productSize"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateProductSizeAsync(ProductSizeCreateDTO productSizeCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var productSize1 = await _repo.CreateProductSizeAsync(productSizeCreateDTO);
                    if (productSize1)
                    {
                        return StatusCode(200, new
                        { 
                            Message = "Create productSize " + ok,
                            Data = productSize1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        { 
                            Message = "Server error",
                            
                        });
                    }

                }
                return StatusCode(400, new
                { 
                    Message = "Dont't accept empty information!",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {                  
                    Message = "An error occurred: " + ex.Message
                });
            }


        }




        [HttpPut]
        public async Task<ActionResult> UpdateProductSizeAsync(ProductSizeDTO productSizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkProductSize = await _repo.CheckProductSizeExistAsync(productSizeDTO.ProductSizeId);
            if (checkProductSize == true)
            {
                var productSize1 = await _repo.UpdateProductSizeAsync(productSizeDTO);
                if (productSize1)
                {
                    return StatusCode(200, new
                    {  
                        Message = "Update productSize" + ok,
                        Data = productSize1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    { 
                        Message = "Server error",                       
                    });
                }
            }
            else
            {
                return StatusCode(400, new
                { 
                    Message = "There already exists a productSize with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {
                Message = "Dont't accept empty information!",
            });

        }




[HttpPatch("{productSizeId}")]
        public async Task<ActionResult> DeleteProductSizeAsync(string productSizeId)
        {
            var productSize1 = await _repo.DeleteProductSizeAsync(productSizeId);
            if (productSize1)
            {
                return StatusCode(200, new
                {
                    Message = "Delete productSize " + ok,

                });
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

    }
}
