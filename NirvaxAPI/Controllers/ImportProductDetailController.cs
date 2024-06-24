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
        private readonly IConfiguration _config;
        private readonly IImportProductDetailRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductDetailController(IConfiguration config, IImportProductDetailRepository repo)
        {
            _config = config;
            _repo = repo;
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
                    Result = true,
                    Message = "Get list Import Product Detail " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any Import Product Detail"
            });
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
                    Result = true,
                    Message = "Get list Import Product Detail " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any Import Product Detail"
            });
        }

        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateImportProductDetailAsync(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var importProductDetail = await _repo.CreateImportProductDetailAsync(importId, importProductDetailDTO);

                if (importProductDetail == true)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Create Import Product Detail " + ok,
                        Data = importProductDetail
                    });
                }

                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Find fail",
                        Message = notFound + "any Import Product Detail"
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
        //  [Authorize]
        public async Task<ActionResult> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var importProductDetail = await _repo.UpdateImportProductDetailAsync(importId, importProductDetailDTO);

                if (importProductDetail == true)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Update Import Product Detail " + ok,
                        Data = importProductDetail
                    });
                }

                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Find fail",
                        Message = notFound + "any Import Product Detail"
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


    }
}
