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
        private readonly IConfiguration _config;
        private readonly IImportProductRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductController(IConfiguration config, IImportProductRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetAllImportProduct(int warehouseId,DateTime? from, DateTime? to)
        {
            var list = await _repo.GetAllImportProduct(warehouseId,from, to);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list import product " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any import product"
            });
        }


        [HttpGet("{importId}")]
        //  [Authorize]
        public async Task<ActionResult> GetImportProductById(int importId)
        {
            var checkSizeExist = await _repo.CheckImportProductExist(importId);
            if (checkSizeExist == true)
            {
                var importProduct = await _repo.GetImportProductById(importId);


                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get import product by id" + ok,
                    Data = importProduct
                });


            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any import product"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateImportProduct(ImportProductDTO importProductDTO)
        {
            try { 
                var importProduct1 = await _repo.CreateImportProduct(importProductDTO);
                if (importProduct1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Create import product " + ok,
                        Data = importProduct1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        Result = true,
                        Message = "Server error",
                        Data = ""
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
        public async Task<ActionResult> UpdateImportProduct(ImportProductDTO importProductDTO)
        {
           
                var importProduct1 = await _repo.UpdateImportProduct(importProductDTO);
                if (importProduct1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update import product" + ok,
                        Data = importProduct1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        Result = true,
                        Message = "Server error",
                        Data = ""
                    });
                }
          

        }

   
        
    }
}
