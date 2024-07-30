using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ImportProductController : ControllerBase
    {
        private readonly IImportProductRepository _repo;
        
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductController(IImportProductRepository repo)
        {
            _repo = repo;
           
        }
        

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetAllImportProductAsync(int warehouseId,DateTime? from, DateTime? to)
        {
            var list = await _repo.GetAllImportProductAsync(warehouseId,from, to);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list import product " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any import product"
                    });
                }
        }


        [HttpGet("{importId}")]
        //  [Authorize]
        public async Task<ActionResult> GetImportProductByIdAsync(int importId)
        {
            var checkSizeExist = await _repo.CheckImportProductExistAsync(importId);
            if (checkSizeExist == true)
            {
                var importProduct = await _repo.GetImportProductByIdAsync(importId);
                return StatusCode(200, new
                {
                    Message = "Get import product by id" + ok,
                    Data = importProduct
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    Message = notFound + "any import product"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var importProduct1 = await _repo.CreateImportProductAsync(importProductCreateDTO);
                    return StatusCode(200, new
                    {
                        Message = "Create import product " + ok,
                        Data = importProduct1
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {


                        Message = "Dont't accept empty information!",
                    });
                }
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

        [HttpPut]
        public async Task<ActionResult> UpdateImportProductAsync(ImportProductDTO importProductDTO)
        {
            try
            {          
            if (ModelState.IsValid)
            {
                var importProduct1 = await _repo.UpdateImportProductAsync(importProductDTO);
                return StatusCode(200, new
                {
                    Message = "Update import product" + ok,
                    Data = importProduct1
                });
            }
            else
            {
                return StatusCode(400, new
                {
                    Message = "Dont't accept empty information!",
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
        public async Task<ActionResult> ViewImportProductStatisticsAsync(int warehouseId, int importId, int ownerId)
        {
            var total = await _repo.ViewImportProductStatisticsAsync(warehouseId);
            var total2 = await _repo.ViewNumberOfProductByImportStatisticsAsync(importId, ownerId);
            var total3 = await _repo.ViewPriceByImportStatisticsAsync(importId, ownerId);
            var total4 = await _repo.QuantityWarehouseStatisticsAsync(ownerId);

            return StatusCode(200, new
            {
                totalImportProduct = total,
                totalProductByImport = total2,
                totalPriceByImport = total3,
                totalQuantityByImport = total4
            });
        }

    }
}
