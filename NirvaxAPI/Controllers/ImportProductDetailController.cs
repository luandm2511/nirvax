using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
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
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductDetailController(IImportProductDetailRepository repo, IProductSizeRepository repoProdSize)
        {
            _repo = repo;
            _repoProdSize = repoProdSize;
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


     


    }
}
