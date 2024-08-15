using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ImportProductDetailController : ControllerBase
    {
        private readonly IImportProductDetailRepository _repo;
        private readonly IProductSizeRepository _repoProdSize;
        private readonly IImportProductRepository _repoImport;
        private readonly ITransactionRepository _transactionRepository;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductDetailController(ITransactionRepository transactionRepository, IImportProductDetailRepository repo, IProductSizeRepository repoProdSize, IImportProductRepository repoImport)
        {
            _repo = repo;
            _repoProdSize = repoProdSize;
            _repoImport = repoImport;
            _transactionRepository = transactionRepository;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProductDetail>>> GetAllImportProductDetailAsync()
        {
            var list = await _repo.GetAllImportProductDetailAsync();
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list Import Product Detail " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any Import Product Detail"
                    });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProductDetail>>> GetAllImportProductDetailByImportIdAsync(int importId)
        {
            var list = await _repo.GetAllImportProductDetailByImportIdAsync(importId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {

                        Message = "Get list Import Product Detail " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any Import Product Detail"
                    });
                }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateImportProductDetailAsync(int ownerId, int importId ,List<ImportProductDetailUpdateDTO> importProductDetail)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                if (ModelState.IsValid)
                {
                    await _repoProdSize.UpdateProductSizeByImportDetailAsync(ownerId,importProductDetail);
                    await _repo.UpdateImportProductDetailAsync(importId, importProductDetail);
                    await _repoImport.UpdateQuantityAndPriceImportProductAsync(importId);
                    await _transactionRepository.CommitTransactionAsync();
                    return StatusCode(200, new
                    {
                        Message = "Update import product detail " + ok,
                     //   Data = importProduct1
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
                    
                    Message = "An error occurred: " + ex.Message
                });
            }

        }



    }
}
