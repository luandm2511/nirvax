using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITransactionRepository _transactionRepository;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductController(ITransactionRepository transactionRepository, IImportProductRepository repo, IProductSizeRepository repoProdSize, IImportProductDetailRepository repoImport)
        {
            _repo = repo;
            _repoProdSize = repoProdSize;
            _repoImport = repoImport;
            _transactionRepository = transactionRepository;
        }


        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetAllImportProductAsync(int ownerId, DateTime? from, DateTime? to)
        {
            var list = await _repo.GetAllImportProductAsync(ownerId, from, to);
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
                return NoContent();

            }
        }


        [HttpGet("{importId}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> GetImportProductByIdAsync(int importId)
        {

            var importProduct = await _repo.GetImportProductByIdAsync(importId);
            if (importProduct != null)
            {
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
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> CreateImportProductAsync(int ownerId, string origin, List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                if (ModelState.IsValid)
                {
                    ImportProductCreateDTO importProduct = new ImportProductCreateDTO()
                    {
                        OwnerId = ownerId,
                        ImportDate = DateTime.Now,
                        Origin = origin,
                        Quantity = 0,
                        TotalPrice = 0
                    };
                    await _repoProdSize.CreateProductSizeAsync(ownerId, importProductDetailDTO);
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
                        Message = "Don't accept empty information!",
                    });
                }
            }
            catch (Exception)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, new
                {

                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }

        [HttpPut]
        [Authorize(Roles = "Owner")]
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
                        Message = "Don't accept empty information!",
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }


        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> ViewImportProductStatisticsAsync(int ownerId)
        {
            try
            {
                var total = await _repo.ViewImportProductStatisticsAsync(ownerId);
                return StatusCode(200, new
                {
                    Data = total,
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> ViewWeeklyImportProductAsync(int ownerId)
        {
            try
            {
                var total = await _repo.ViewWeeklyImportProductAsync(ownerId);


                return StatusCode(200, new
                {
                    Data = total
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }


    }
}
