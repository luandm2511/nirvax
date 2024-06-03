using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageService _service;

        public CategoryController(ICategoryRepository repository, IMapper mapper, IImageService service)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var categories = _repository.GetAllCategory();
                return Ok(categories);
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
                var category = _repository.GetCategoryById(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category not found." });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var category = _mapper.Map<Category>(categoryDto);
                var check = _repository.CheckCategory(category);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category name has been duplicated." });
                }

                if (categoryDto.ImageFile != null)
                {
                    var imagePath = _service.SaveImage(categoryDto.ImageFile,"categories");
                    category.Image = imagePath;
                }

                var result = _repository.CreateCategory(category);
                if (result)
                {
                    return Ok(new { message = "Category added successfully." });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the category." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var category = _repository.GetCategoryById(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category not found." });
                }

                _mapper.Map(categoryDto, category);
                var check = _repository.CheckCategory(category);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category name has been duplicated." });
                }

                if (categoryDto.ImageFile != null)
                {
                    try
                    {
                        var imagePath = _service.SaveImage(categoryDto.ImageFile, "categories");
                        // Xóa ảnh cũ trước khi cập nhật ảnh mới
                        if (!string.IsNullOrEmpty(category.Image))
                        {
                            _service.DeleteImage(category.Image);
                        }
                        category.Image = imagePath;
                    }
                    catch (InvalidOperationException ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                    
                }

                var result = _repository.Update(category);
                if (result)
                {
                    return Ok(new { message = "Category updated successfully." });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update the category." });
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
                var category = _repository.GetCategoryById(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category not found." });
                }
                var result = _repository.DeleteCategory(category);
                if (result)
                {
                    return Ok(new { message = "Category deleted successfully." });
                }
                return NotFound(new { message = "Category not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }

}
