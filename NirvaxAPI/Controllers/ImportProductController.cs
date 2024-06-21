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
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetAllImportProductAsync(int warehouseId,DateTime? from, DateTime? to)
        {
            var list = await _repo.GetAllImportProductAsync(warehouseId,from, to);
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
        public async Task<ActionResult> GetImportProductByIdAsync(int importId)
        {
            var checkSizeExist = await _repo.CheckImportProductExistAsync(importId);
            if (checkSizeExist == true)
            {
                var importProduct = await _repo.GetImportProductByIdAsync(importId);


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
        public async Task<ActionResult> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var importProduct1 = await _repo.CreateImportProductAsync(importProductCreateDTO);
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
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "Dont't accept empty information!",
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

        [HttpPut]
        public async Task<ActionResult> UpdateImportProductAsync(ImportProductDTO importProductDTO)
        {
            if (ModelState.IsValid)
            {
                var importProduct1 = await _repo.UpdateImportProductAsync(importProductDTO);
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

            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Dont't accept empty information!",
            });


        }

   
        
    }
}
