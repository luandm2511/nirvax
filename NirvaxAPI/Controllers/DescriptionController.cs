using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DescriptionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDescriptionRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public DescriptionController(IConfiguration config, IDescriptionRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Description>>> GetAllDescriptions(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllDescriptions(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list description " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any description"
            });
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetDescriptionById(int sizeId)
        {
            var checkSizeExist = await _repo.CheckDescriptionExist(sizeId);
            if (checkSizeExist == true)
            {
                var description = await _repo.GetDescriptionById(sizeId);


                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get description by id" + ok,
                    Data = description
                });


            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any description"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateDesctiption(DescriptionDTO descriptionDTO)
        {
            if (ModelState.IsValid)
            {

                var checkDescription = await _repo.CheckDescription(descriptionDTO);
            if (checkDescription == true)
            {
                var description1 = await _repo.CreateDesctiption(descriptionDTO);
                if (description1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Create description " + ok,
                        Data = description1
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
            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a description with that information",
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

        [HttpPut]
        public async Task<ActionResult> UpdateDesctiption(DescriptionDTO descriptionDTO)
        {
            var checkDescription = await _repo.CheckDescription(descriptionDTO);
            if (checkDescription == true)
            {
                var description1 = await _repo.UpdateDesctiption(descriptionDTO);
                if (description1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update description" + ok,
                        Data = description1
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
                Message = "The name description is already exist",
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> DeleteDesctiption(int sizeId)
        {
            var description1 = await _repo.DeleteDesctiption(sizeId);
            if (description1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Delete description " + ok,

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
