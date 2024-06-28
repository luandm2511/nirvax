using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateParentController : ControllerBase
    {
        private readonly ICateParentRepository _repository;
        private readonly IMapper _mapper;

        public CateParentController(ICateParentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _repository.GetAllCategoryParentAsync();
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
                var category = await _repository.GetCategoryParentByIdAsync(id);
                if (category == null || category.Isdelete == true)
                {
                    return NotFound(new { message = "Category parent not found." });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CateParentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var cateparent = _mapper.Map<CategoryParent>(dto);
                var check = await _repository.CheckCategoryParentAsync(cateparent);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category name has been duplicated." });
                }

                await _repository.CreateCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] CateParentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var cateparent = await _repository.GetCategoryParentByIdAsync(id);
                if (cateparent == null || cateparent.Isdelete == true)
                {
                    return NotFound(new { message = "Category parent not found." });
                }

                _mapper.Map(dto, cateparent);
                var check = await _repository.CheckCategoryParentAsync(cateparent);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The category parent name has been duplicated." });
                }

                await _repository.CheckCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent updated successfully." }); 
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
                var cateparent = await _repository.GetCategoryParentByIdAsync(id);
                if (cateparent == null || cateparent.Isdelete == true)
                {
                    return NotFound(new { message = "Category parent not found." });
                }
                var result = await _repository.CheckCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
