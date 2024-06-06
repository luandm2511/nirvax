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
        public async Task<ActionResult> GetWarehouseIdByOwnerId(int ownerId)
        {
            
                var warehouse = await _repo.GetWarehouseIdByOwnerId(ownerId);
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
        public async Task<ActionResult> GetWarehouseByOwner(int ownerId)
        {
          
                var wh = await _repo.GetWarehouseById(ownerId);

                if (wh != null)
                {
                   var result = await _repo.UpdateQuantityAndPriceWarehouse(ownerId);
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
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetWarehouseByImportProduct(int ownerId, int page, int pageSize)
        {
            var list = await _repo.GetWarehouseByImportProduct(ownerId,  page,  pageSize);
            if (list.Any())
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouse(ownerId);
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
        public async Task<ActionResult<IEnumerable<WarehouseDetail>>> GetAllWarehouseDetail(int ownerId, int page, int pageSize)
        {
            var list = await _repo.GetAllWarehouseDetail(ownerId, page, pageSize);
            if (list.Any())
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouse(ownerId);
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
        public async Task<ActionResult> CreateWarehouse(WarehouseDTO warehouseDTO)
        {
            try
            {
                var warehouse = await _repo.CreateWarehouse(warehouseDTO);

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
        public async Task<ActionResult> UpdateWarehouse(WarehouseDTO warehouseDTO)
        {
            try
            {
                var size1 = await _repo.UpdateWarehouse(warehouseDTO);
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
        public async Task<ActionResult> ViewCountImportStatistics(int warehouseId)
        {
            var number = await _repo.ViewCountImportStatistics(warehouseId);
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
        public async Task<ActionResult> ViewNumberOfProductByImportStatistics(int importId, int ownerId)
        {
            var number = await _repo.ViewNumberOfProductByImportStatistics(importId, ownerId);
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
        public async Task<ActionResult> ViewPriceByImportStatistics(int importId, int ownerId)
        {
            var number = await _repo.ViewPriceByImportStatistics(importId, ownerId);
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
        public async Task<ActionResult> QuantityWarehouseStatistics(int ownerId)
        {
            var number = await _repo.QuantityWarehouseStatistics(ownerId);
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
