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
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
                    return StatusCode(404,new { message = "The category parent has been not found." });
                }
                return Ok(category);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CateParentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            try
            {
                var cateparent = _mapper.Map<CategoryParent>(dto);
                var check = await _repository.CheckCategoryParentAsync(cateparent);
                if (!check)
                {
                    return StatusCode(406, new { message = "The category name has been duplicated." });
                }

                await _repository.CreateCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent added successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
                    return StatusCode(400,new { message = "The category parent has been not found." });
                }

                _mapper.Map(dto, cateparent);
                var check = await _repository.CheckCategoryParentAsync(cateparent);
                if (!check)
                {
                    return StatusCode(406, new { message = "The category parent name has been duplicated." });
                }

                await _repository.CheckCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent updated successfully." }); 
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cateparent = await _repository.GetCategoryParentByIdAsync(id);
                if (cateparent == null || cateparent.Isdelete == true)
                {
                    return StatusCode(404,new { message = "The category parent has been not found." });
                }
                await _repository.DeleteCategoryParentAsync(cateparent);
                return Ok(new { message = "Category parent deleted successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCateParent(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return StatusCode(400,new { message = "Keyword must not be empty" });
                }

                var categoryParents = await _repository.SearchCateParentsAsync(keyword);
                return Ok(categoryParents);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }
    }
}
