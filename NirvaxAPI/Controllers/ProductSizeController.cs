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
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetAllProductSizes(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllProductSizes(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200,new
                {
                    Result = true,
                    Message = "Get list productSize " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any productSize"
            });
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSizeByProductId(int productId)
        {
            var list = await _repo.GetProductSizeByProductId(productId);
            if (list.Any())
            {
                return StatusCode(200,new
                {
                    Result = true,
                    Message = "Get list productSize " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any productSize"
            });
        }




        [HttpGet("{productSizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetProductSizeById(string productSizeId)
        {
            var checkProductSize = await _repo.CheckProductSizeById(productSizeId);
            if (checkProductSize == true)
            {
                var productSize = await _repo.GetProductSizeById(productSizeId);
                return StatusCode(200,new
                {
                    Result = true,
                    Message = "Get productSize by id " + ok,
                    Data = productSize
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any productSize"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateProductSize(ProductSizeDTO productSizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkProductSize = await _repo.CheckProductSize(productSizeDTO);
            if (checkProductSize == true)
            {
                var productSize1 = await _repo.CreateProductSize(productSizeDTO);
                if (productSize1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Create productSize " + ok,
                        Data = productSize1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        Result = true,
                        Message = "Server error",
                        Data = ""
                    });
                }
            }

                else
                {
                    return StatusCode(400, new
                    {
                        StatusCode = 400,
                        Result = false,
                        Message = "There already exists a staff with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Dont't accept empty information!",
            });


        }




        [HttpPut]
        public async Task<ActionResult> UpdateProductSize(ProductSizeDTO productSizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkProductSize = await _repo.CheckProductSizeExist(productSizeDTO.ProductSizeId);
            if (checkProductSize == true)
            {
                var productSize1 = await _repo.UpdateProductSize(productSizeDTO);
                if (productSize1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update productSize" + ok,
                        Data = productSize1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        Result = true,
                        Message = "Server error",
                        Data = ""
                    });
                }
            }
            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a productSize with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Dont't accept empty information!",
            });

        }




[HttpPatch("{productSizeId}")]
        public async Task<ActionResult> DeleteProductSize(string productSizeId)
        {
            var productSize1 = await _repo.DeleteProductSize(productSizeId);
            if (productSize1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Delete productSize " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

    }
}
