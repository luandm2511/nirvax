using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
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
                    return NotFound(new { message = "Category is not found." });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new { message = "Please pass the valid data." });
                }
                var category = _mapper.Map<Category>(categoryDto);
                var check = await _repository.CheckCategoryAsync(category);
                if (!check)
                {
                    return StatusCode(406, new { message = "The category name has been duplicated." });
                }

                await _repository.CreateCategoryAsync(category);
                return Ok(new { message = "Category added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new { message = "Please pass the valid data." });
                }
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null || category.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Category is not found." });
                }

                _mapper.Map(categoryDto, category);
                var check = await _repository.CheckCategoryAsync(category);
                if (!check)
                {
                    return StatusCode(406, new { message = "The category name has been duplicated." });
                }

                await _repository.UpdateAsync(category);
                return Ok(new { message = "Category updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null || category.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Category is not found." });
                }
                await _repository.DeleteCategoryAsync(category);
                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategories(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return StatusCode(400,new { message = "Keyword must not be empty" });
                }

                var categories = await _repository.SearchCategoriesAsync(keyword);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }

}
