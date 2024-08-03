using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
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
        private readonly IProductSizeRepository _repoProdSize;
        private readonly IImportProductDetailRepository _repoImport;
        private readonly IWarehouseDetailRepository _repoWh;
        private readonly ITransactionRepository _transactionRepository;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductController(ITransactionRepository transactionRepository, IWarehouseDetailRepository repoWh, IImportProductRepository repo, IProductSizeRepository repoProdSize, IImportProductDetailRepository repoImport)
        {
            _repo = repo;
            _repoProdSize = repoProdSize;
            _repoImport = repoImport;
            _repoWh = repoWh;
            _transactionRepository = transactionRepository;
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
        public async Task<ActionResult> CreateImportProductAsync(int ownerId,int warehouseId, string origin, List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try {
                if (ModelState.IsValid)
                {
                    ImportProductCreateDTO importProduct = new ImportProductCreateDTO()
                    {
                        WarehouseId = warehouseId,
                        ImportDate = DateTime.Now,
                        Origin = origin,
                        Quantity = 0,
                        TotalPrice = 0
                    };
                    await _repoProdSize.CreateProductSizeAsync(ownerId, importProductDetailDTO);             
                    await _repoWh.CreateWarehouseDetailAsync(warehouseId, importProductDetailDTO);
                    var importProduct1 = await _repo.CreateImportProductAsync(importProduct);              
                    await _repoImport.CreateImportProductDetailAsync(importProduct1.ImportId, importProductDetailDTO);
                    await _repo.UpdateQuantityAndPriceImportProductAsync(importProduct1.ImportId);               
                    await _transactionRepository.CommitTransactionAsync();
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
                await _transactionRepository.RollbackTransactionAsync();
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
