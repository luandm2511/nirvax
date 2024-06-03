using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _repository;
        private readonly ICategoryRepository _cate;
        private readonly IMapper _mapper;
        private readonly IImageService _service;

        public BrandController(IBrandRepository repository, IMapper mapper, IImageService service, ICategoryRepository cate)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
            _cate = cate;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var brands = _repository.GetAllBrand();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var brand = _repository.GetBrandById(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return NotFound(new { message = "Brand not found." });
                }
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("GetByCategory/{cate_id}")]
        public IActionResult GetByCategory(int cate_id)
        {
            try
            {
                var brands = _repository.GetBrandsByCategory(cate_id);
                if (brands == null)
                {
                    return NotFound(new { message = "Brand not found." });
                }
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromForm] BrandDTO brandDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var brand = _mapper.Map<Brand>(brandDto);
                var check = _repository.CheckBrand(brand);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The brand name has been duplicated." });
                }            
                
                if (brandDto.ImageFile != null)
                {
                    var imagePath = _service.SaveImage(brandDto.ImageFile, "brands");
                    brand.Image = imagePath;
                }

                var result = _repository.CreateBrand(brand);
                if (result)
                {
                    return Ok(new { message = "Brand added successfully." });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the brand." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] BrandDTO brandDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }

                var brand = _repository.GetBrandById(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return NotFound(new { message = "Brand not found." });
                }    

                _mapper.Map(brandDto, brand);
                var check = _repository.CheckBrand(brand);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The brand name has been duplicated." });
                }

                if (brandDto.ImageFile != null)
                {
                    try
                    {
                        var imagePath = _service.SaveImage(brandDto.ImageFile, "brands");
                        // Xóa ảnh cũ trước khi cập nhật ảnh mới
                        if (!string.IsNullOrEmpty(brand.Image))
                        {
                            _service.DeleteImage(brand.Image);
                        }
                        brand.Image = imagePath;
                    }
                    catch (InvalidOperationException ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }

                }

                var result = _repository.Update(brand);
                if (result)
                {
                    return Ok(new { message = "Brand updated successfully." });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update the brand." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var brand = _repository.GetBrandById(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return NotFound(new { message = "Brand not found." });
                }
                var result = _repository.DeleteBrand(brand);
                if (result)
                {
                    return Ok(new { message = "Brand deleted successfully." });
                }
                return NotFound(new { message = "Brand not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
