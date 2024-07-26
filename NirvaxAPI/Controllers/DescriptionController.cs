using AutoMapper;
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
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionRepository _repo;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IImageRepository _imageRepository;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";
        private readonly IMapper _mapper;

        public DescriptionController(IDescriptionRepository repo, IImageRepository imageRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _repo = repo;
            _imageRepository = imageRepository;
            _mapper = mapper;
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

                    Message = "Get list of descriptions " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {

                Message = notFound + "any description"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Description>>> GetDescriptionForUserAsync(string? searchQuery)
        {
            var description = await _repo.GetDescriptionForUserAsync(searchQuery);
            if (description.Any())
            {
                return StatusCode(200, new
                {

                    Message = "Get descriptions" + ok,
                    Data = description
                });
            }
            return StatusCode(404, new
            {

                Message = notFound + "any description"
            });
        }


        [HttpGet("{descriptionId}")]
        //  [Authorize]
        public async Task<ActionResult> GetDescriptionByIdAsync(int descriptionId)
        {
            var checkDescriptionExist = await _repo.CheckDescriptionExistAsync(descriptionId);
            if (checkDescriptionExist == true)
            {
                var description = await _repo.GetDescriptionByIdAsync(descriptionId);


                return StatusCode(200, new
                {

                    Message = "Get description by id" + ok,
                    Data = description
                });


            }

            return StatusCode(404, new
            {

                Message = notFound + "any description"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateDesctiptionAsync([FromForm] DescriptionCreateDTO descriptionCreateDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try {
                if (ModelState.IsValid)
                {

                    var checkDescription = await _repo.CheckDescriptionAsync(0, descriptionCreateDTO.Title, descriptionCreateDTO.Content);
                    if (checkDescription == true)
                    {
                        var description1 = await _repo.CreateDesctiptionAsync(descriptionCreateDTO);
                        foreach (var link in descriptionCreateDTO.ImageLinks)
                        {
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = link,
                                DescriptionId = description1.DescriptionId
                            };
                            await _imageRepository.AddImagesAsync(image);

                        }
                        await _transactionRepository.CommitTransactionAsync();

                        return StatusCode(200, new
                        {
                            Message = "Create description " + ok,
                            Data = description1
                        });

                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a description with that information",
                        });
                    }

                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Description!",
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

        [HttpPut]
        public async Task<ActionResult> UpdateDesctiptionAsync([FromForm] DescriptionDTO descriptionDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid) {

                    var checkDescription = await _repo.CheckDescriptionAsync(descriptionDTO.DescriptionId, descriptionDTO.Title, descriptionDTO.Content);
                    if (checkDescription == true)
                    {
                        var description1 = await _repo.UpdateDesctiptionAsync(descriptionDTO);
                        IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetByDescriptionAsync(descriptionDTO.DescriptionId);
                        foreach (BusinessObject.Models.Image img in images)
                        {
                            // Xóa ảnh cũ trước khi cập nhật ảnh mới
                            await _imageRepository.DeleteImagesAsync(img);

                        }
                        foreach (var link in descriptionDTO.ImageLinks)
                        {
                            // Save image information to database
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = link,
                                DescriptionId = description1.DescriptionId
                            };
                            await _imageRepository.AddImagesAsync(image);
                        }
                        await _transactionRepository.CommitTransactionAsync();

                        return StatusCode(200, new
                        {
                            Message = "Update description" + ok,
                            Data = description1
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a description with that information!",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Description!",
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

        [HttpPatch("{descriptionId}")]
        public async Task<ActionResult> DeleteDesctiptionAsync(int descriptionId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();

            try
            { 
            IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetByDescriptionAsync(descriptionId);
            foreach (BusinessObject.Models.Image img in images)
            {
                // Xóa ảnh cũ trước khi xóa ảnh mới
                await _imageRepository.DeleteImagesAsync(img);
            }
            var description = await _repo.DeleteDesctiptionAsync(descriptionId);
                if (description)
                {
                    await _transactionRepository.CommitTransactionAsync();

                    return StatusCode(200, new
                    {
                        Message = "Delete description " + ok,
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
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
