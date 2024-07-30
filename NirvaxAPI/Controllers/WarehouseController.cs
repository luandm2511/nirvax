using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Pipelines.Sockets.Unofficial.Buffers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseRepository _repo;
       

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public WarehouseController(IWarehouseRepository repo)
        {
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
                    
                    Message = "Get warehouse by id" + ok,
                    Data = warehouse
                });
            }
            else
            {
                return StatusCode(404, new
                {                 
                    Message = notFound + "any warehouse"
                });
            }

        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetWarehouseByOwnerAsync(int ownerId)
        {
            try { 
                var wh = await _repo.GetWarehouseByOwnerIdAsync(ownerId);

                if (wh != null)
                {
                    var result = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                        return StatusCode(200, new
                        {
                            Message = "Get warehouse by owner" + ok,
                            Data = wh,
                            result.TotalPrice,
                            result.TotalQuantity
                        });                  
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = "Error for total quantity and total price of warehouse"
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
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize)
        {
            try {
                
                var list = await _repo.GetWarehouseByImportProductAsync(ownerId,  page,  pageSize);
            if (list.Any())
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                if(numberOfWarehouse != null)
                {
                 
                        return StatusCode(200, new
                    {  
                        Message = "Get list Warehouse " + ok,
                        Data = list,
                        numberOfWarehouse.TotalPrice,
                        numberOfWarehouse.TotalQuantity
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {  
                        Message = "Can't get quantity and price of warehouse!",
                    });
                }
            }
            return StatusCode(404, new
            {
                Message = notFound + "any Warehouse"
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

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<WarehouseDetail>>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllWarehouseDetailAsync(ownerId, page, pageSize);
            if (list.Any() && list!= null)
            {
                var numberOfWarehouse = await _repo.UpdateQuantityAndPriceWarehouseAsync(ownerId);
                if (numberOfWarehouse != null)
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list of Warehouse " + ok,
                        Data = list,
                        numberOfWarehouse.TotalPrice,
                        numberOfWarehouse.TotalQuantity
                    });
                } else
                {
                    return StatusCode(400, new
                    { 
                       
                        
                        Message = "Can't get quantity and price of warehouse!",
                    });
                }
            }
            return StatusCode(404, new
            {
               
                Message = "Warehouse is empty!"
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






        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var warehouse = await _repo.CreateWarehouseAsync(warehouseCreateDTO);
                       return StatusCode(200, new
                       { 
                          Message = "Create Warehouse" + ok,
                          Data = warehouse
                        });           
                }
                else
                {
                    return StatusCode(400, new
                    {         
                        Message = "Please enter valid Warehouse!",
                    });
                }
             }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                  
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
                        Message = "Update warehouse " + ok,
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
