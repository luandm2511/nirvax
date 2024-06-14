using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWarehouseRepository _repo;
       

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public WarehouseController(IConfiguration config, IWarehouseRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetWarehouseIdByOwnerIdAsync(int ownerId)
        {
            
                var warehouse = await _repo.GetWarehouseIdByOwnerIdAsync(ownerId);
            if (warehouse != null)
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get warehouse by id" + ok,
                    Data = warehouse
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any warehouse"
                });
            }
           
        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetWarehouseByOwnerAsync(int ownerId)
        {
          
                var wh = await _repo.GetWarehouseByIdAsync(ownerId);

                if (wh != null)
                {
                   var result = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                   if (result != null)
                   {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get warehouse by owner" + ok,
                        Data = wh,
                        result.TotalPrice,
                        result.TotalQuantity
                    });
                   }
                }
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = "Error for total quantity and total price of warehouse"
                });
           
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize)
        {
            var list = await _repo.GetWarehouseByImportProductAsync(ownerId,  page,  pageSize);
            if (list.Any())
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                if(numberOfWarehouse != null)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list Warehouse " + ok,
                        Data = list,
                        numberOfWarehouse.TotalPrice,
                        numberOfWarehouse.TotalQuantity
                    });
                }
               
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Warehouse"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<WarehouseDetail>>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize)
        {
            var list = await _repo.GetAllWarehouseDetailAsync(ownerId, page, pageSize);
            if (list.Any())
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                if (numberOfWarehouse != null)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list Warehouse " + ok,
                        Data = list,
                        numberOfWarehouse.TotalPrice,
                        numberOfWarehouse.TotalQuantity
                    });
                }
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Warehouse"
            });
        }






        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO)
        {
            try
            {
                var warehouse = await _repo.CreateWarehouseAsync(warehouseCreateDTO);

                if (warehouse == true)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Create Warehouse" + ok,
                        Data = warehouse
                    });
                }

                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any Warehouse"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWarehouseAsync(WarehouseDTO warehouseDTO)
        {
            try
            {
                var size1 = await _repo.UpdateWarehouseAsync(warehouseDTO);
                if (size1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update warehouse " + ok,

                    });
                }
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = badRequest,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }
           

        }

        [HttpGet]
        public async Task<ActionResult> ViewCountImportStatisticsAsync(int warehouseId)
        {
            var number = await _repo.ViewCountImportStatisticsAsync(warehouseId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId)
        {
            var number = await _repo.ViewNumberOfProductByImportStatisticsAsync(importId, ownerId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> ViewPriceByImportStatisticsAsync(int importId, int ownerId)
        {
            var number = await _repo.ViewPriceByImportStatisticsAsync(importId, ownerId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> QuantityWarehouseStatisticsAsync(int ownerId)
        {
            var number = await _repo.QuantityWarehouseStatisticsAsync(ownerId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

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
