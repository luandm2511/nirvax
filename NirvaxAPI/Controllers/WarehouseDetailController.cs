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
        private readonly IConfiguration _config;
        private readonly IWarehouseDetailRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public WarehouseDetailController(IConfiguration config, IWarehouseDetailRepository repo)
        {
            _config = config;
            _repo = repo;
        }



        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<WarehouseDetailFinalDTO>>> GetAllWarehouseDetailByProductSizeAsync(int warehouseId, int page, int pageSize)
        {
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
                Status = "Find fail",
                Message = notFound + "any Warehouse detail"
            });
        }






        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
            if (ModelState.IsValid)
            {
                var warehouse = await _repo.CreateWarehouseDetailAsync(warehouseDetailDTO);

            if (warehouse == true)
            {
                return StatusCode(200, new
                {
                    
                    Message = "Create Warehouse detail" + ok,
                    Data = warehouse
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Warehouse detail"
            });
            }

            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "Dont't accept empty information!",
            });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
                if (ModelState.IsValid)
                {
                    var size1 = await _repo.UpdateWarehouseDetailAsync(warehouseDetailDTO);
            if (size1)
            {
                return StatusCode(200, new
                {

                    
                    Message = "Update Warehouse detail" + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = badRequest,
            });
                }

                return StatusCode(400, new
                {
                    StatusCode = 400,
                    
                    Message = "Dont't accept empty information!",
                });

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
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = badRequest,
            });

        }


    }
}

