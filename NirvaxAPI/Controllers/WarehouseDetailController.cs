using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class WarehouseDetailController : ControllerBase
    {
        private readonly IWarehouseDetailRepository _repo;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public WarehouseDetailController(IWarehouseDetailRepository repo) 
        { 
            _repo = repo;
        }



        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<WarehouseDetailFinalDTO>>> GetAllWarehouseDetailByProductSizeAsync(int warehouseId, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllWarehouseDetailByProductSizeAsync(warehouseId, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {                 
                    Message = "Get list Warehouse detail " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {            
                Message = notFound + "any Warehouse detail"
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
        public async Task<ActionResult> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var wh = await _repo.UpdateWarehouseDetailAsync(warehouseDetailDTO);
                    if (wh)
                    {           
                        return StatusCode(200, new
                        {
                            Message = "Update Warehouse detail" + ok,
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
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Warehouse Detail!",
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
        public async Task<ActionResult> SumOfKindProdSizeStatisticsAsync(int warehouseId)
        {
            var number = await _repo.SumOfKindProdSizeStatisticsAsync(warehouseId);
                if (number != null)
                {
                    return StatusCode(200, new
                    {
                        Message = number,
                    });
                }
                else
                {
                    return StatusCode(200, new
                    {
                        Message = 0,
                    });
                }          
        }

    }
}

