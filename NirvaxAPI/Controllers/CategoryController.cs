using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _repository.GetAllCategoryAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
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
        public async Task<IActionResult> Create([FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var category = _mapper.Map<Category>(categoryDto);
                var check = await _repository.CheckCategoryAsync(category);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category name has been duplicated." });
                }

                var result = await _repository.CreateCategoryAsync(category);
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
        public async Task<IActionResult> Update(int id, [FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category not found." });
                }

                _mapper.Map(categoryDto, category);
                var check = await _repository.CheckCategoryAsync(category);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category name has been duplicated." });
                }

                var result = await _repository.UpdateAsync(category);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category not found." });
                }
                var result = await _repository.DeleteCategoryAsync(category);
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
