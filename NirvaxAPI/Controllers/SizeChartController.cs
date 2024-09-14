using AutoMapper;
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
    public class SizeChartController : ControllerBase
    {
        private readonly ISizeChartRepository _repo;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IImageRepository _imageRepository;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public SizeChartController(ISizeChartRepository repo, IImageRepository imageRepository, ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _repo = repo;
            _imageRepository = imageRepository;
        }


        [HttpGet]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> GetAllSizeChartsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var list = await _repo.GetAllSizeChartsAsync(searchQuery, page, pageSize, ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {

                    Message = "Get list of descriptions " + ok,
                    Data = list
                });
            }
            else
            {
                return NoContent();

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSizeChartForUserAsync(string? searchQuery)
        {
            var sizeChart = await _repo.GetSizeChartForUserAsync(searchQuery);
            if (sizeChart.Any())
            {
                return StatusCode(200, new
                {

                    Message = "Get size chart" + ok,
                    Data = sizeChart
                });
            }
            else
            {
                return NoContent();

            }
        }


        [HttpGet("{sizeChartId}")]
        public async Task<ActionResult> GetSizeChartByIdAsync(int sizeChartId)
        {

            var sizeChart = await _repo.GetSizeChartByIdAsync(sizeChartId);

            if (sizeChart != null)
            {
                return StatusCode(200, new
                {

                    Message = "Get size chart by id" + ok,
                    Data = sizeChart
                });


            }
            else
            {
                return StatusCode(404, new
                {

                    Message = notFound + "any size chart"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> CreateSizeChartAsync([FromForm] SizeChartCreateDTO sizeChartDTOCreateDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                if (ModelState.IsValid)
                {

                    var checkSizeChart = await _repo.CheckSizeChartAsync(0, sizeChartDTOCreateDTO.Title, sizeChartDTOCreateDTO.Content, sizeChartDTOCreateDTO.OwnerId);
                    if (checkSizeChart == true)
                    {
                        var description1 = await _repo.CreateSizeChartAsync(sizeChartDTOCreateDTO);
                        foreach (var link in sizeChartDTOCreateDTO.ImageLinks)
                        {
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = link,
                                SizeChartId = description1.SizeChartId
                            };
                            await _imageRepository.AddImagesAsync(image);

                        }
                        await _transactionRepository.CommitTransactionAsync();

                        return StatusCode(200, new
                        {
                            Message = "Create size chart " + ok,
                            Data = description1
                        });

                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a size chart with that information",
                        });
                    }

                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid size chart!",
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
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> UpdateSizeChartAsync([FromForm] SizeChartDTO sizeChartDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid)
                {

                    var checkSizeChart = await _repo.CheckSizeChartAsync(sizeChartDTO.SizeChartId, sizeChartDTO.Title, sizeChartDTO.Content, sizeChartDTO.OwnerId);
                    if (checkSizeChart == true)
                    {
                        var sizeChart = await _repo.UpdateSizeChartAsync(sizeChartDTO);
                        IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetBySizeChartAsync(sizeChartDTO.SizeChartId);
                        foreach (BusinessObject.Models.Image img in images)
                        {
                            await _imageRepository.DeleteImagesAsync(img);

                        }
                        foreach (var link in sizeChartDTO.ImageLinks)
                        {
                            var image = new BusinessObject.Models.Image
                            {
                                LinkImage = link,
                                SizeChartId = sizeChart.SizeChartId
                            };
                            await _imageRepository.AddImagesAsync(image);
                        }
                        await _transactionRepository.CommitTransactionAsync();

                        return StatusCode(200, new
                        {
                            Message = "Update size chart" + ok,
                            Data = sizeChart
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a size chart with that information!",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid size chart!",
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

        [HttpPatch("{sizeChartId}")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> DeleteSizeChartAsync(int sizeChartId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();

            try
            {
                IEnumerable<BusinessObject.Models.Image> images = await _imageRepository.GetBySizeChartAsync(sizeChartId);
                foreach (BusinessObject.Models.Image img in images)
                {
                    await _imageRepository.DeleteImagesAsync(img);
                }
                var sizeChart = await _repo.DeleteSizeChartAsync(sizeChartId);
                if (sizeChart)
                {
                    await _transactionRepository.CommitTransactionAsync();

                    return StatusCode(200, new
                    {
                        Message = "Delete size chart " + ok,
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
            catch (Exception)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }

    }
}
