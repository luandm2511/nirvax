﻿using AutoMapper;
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
        private readonly IConfiguration _config;
        private readonly IDescriptionRepository _repo;
        private readonly IImageRepository _imageRepository;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";
        private readonly IMapper _mapper;

        public DescriptionController(IConfiguration config, IDescriptionRepository repo, IImageRepository imageRepository,  IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Description>>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize)
        {
            try { 
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
                
                Message = notFound + "any description"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetDescriptionByIdAsync(int sizeId)
        {
            try { 
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
                
                Message = notFound + "any description"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateDesctiptionAsync([FromForm]DescriptionCreateDTO descriptionCreateDTO)
        {
            using var transaction = await _repo.BeginTransactionAsync();
            try {
                if (ModelState.IsValid)
                {
                    var des = _mapper.Map<Description>(descriptionCreateDTO);
                    var checkDescription = await _repo.CheckDescriptionAsync(0, descriptionCreateDTO.Title, descriptionCreateDTO.Content);
                    if (checkDescription == true)
                    {
                        var description1 = await _repo.CreateDesctiptionAsync(descriptionCreateDTO);
                        foreach (var link in descriptionCreateDTO.ImageLinks)
                        {
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = link,
                                DescriptionId = des.DescriptionId
                            };
                            await _imageRepository.AddImagesAsync(image);

                        }
                        await _repo.CommitTransactionAsync();

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
                await _repo.RollbackTransactionAsync();
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateDesctiptionAsync([FromForm] DescriptionDTO descriptionDTO)
        {
            using var transaction = await _repo.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid) { 
            var des = _mapper.Map<Description>(descriptionDTO);
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
                                DescriptionId = des.DescriptionId
                            };
                            await _imageRepository.AddImagesAsync(image);
                        }
                        await _repo.CommitTransactionAsync();

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
                            Message = "The name description is already exist",
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
                await _repo.RollbackTransactionAsync();
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> DeleteDesctiptionAsync(int descriptionId)
        {
            using var transaction = await _repo.BeginTransactionAsync();

            try
            { 
            IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetByDescriptionAsync(descriptionId);
            foreach (BusinessObject.Models.Image img in images)
            {
                // Xóa ảnh cũ trước khi xóa ảnh mới
                await _imageRepository.DeleteImagesAsync(img);
            }
            var description1 = await _repo.DeleteDesctiptionAsync(descriptionId);
                if (description1)
                {
                    await _repo.CommitTransactionAsync();

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
                await _repo.RollbackTransactionAsync();
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

    }
}
