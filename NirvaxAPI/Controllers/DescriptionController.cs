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
        public async Task<ActionResult<IEnumerable<Description>>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllDescriptionsAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    
                    Message = "Get list description " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any description"
            });
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetDescriptionByIdAsync(int sizeId)
        {
            var checkSizeExist = await _repo.CheckDescriptionExistAsync(sizeId);
            if (checkSizeExist == true)
            {
                var description = await _repo.GetDescriptionByIdAsync(sizeId);


                return StatusCode(200, new
                {
                    
                    Message = "Get description by id" + ok,
                    Data = description
                });


            }

            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any description"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO)
        {
            if (ModelState.IsValid)
            {

                var checkDescription = await _repo.CheckDescriptionAsync(0,descriptionCreateDTO.Title, descriptionCreateDTO.Content);
            if (checkDescription == true)
            {
                var description1 = await _repo.CreateDesctiptionAsync(descriptionCreateDTO);
                if (description1)
                {
                    return StatusCode(200, new
                    {

                        
                        Message = "Create description " + ok,
                        Data = description1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        
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
                    
                    Message = "There already exists a description with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "Dont't accept empty information!",
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO)
        {
            var checkDescription = await _repo.CheckDescriptionAsync(descriptionDTO.DescriptionId, descriptionDTO.Title, descriptionDTO.Content);
            if (checkDescription == true)
            {
                var description1 = await _repo.UpdateDesctiptionAsync(descriptionDTO);
                if (description1)
                {
                    return StatusCode(200, new
                    {

                        
                        Message = "Update description" + ok,
                        Data = description1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {

                        
                        Message = "Server error",
                        Data = ""
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "The name description is already exist",
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> DeleteDesctiptionAsync(int sizeId)
        {
            var description1 = await _repo.DeleteDesctiptionAsync(sizeId);
            if (description1)
            {
                return StatusCode(200, new
                {

                    
                    Message = "Delete description " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = badRequest,
            });

        }

    }
}
