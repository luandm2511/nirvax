using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _repository;
        private readonly ICategoryRepository _cate;
        private readonly IMapper _mapper;

        public BrandController(IBrandRepository repository, IMapper mapper, ICategoryRepository cate)
        {
            _repository = repository;
            _mapper = mapper;
            _cate = cate;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var brands = await _repository.GetAllBrandAsync();
                return Ok(brands);
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
                var brand = await _repository.GetBrandByIdAsync(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return StatusCode(404, new { message = "Brand is not found." });
                }
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] BrandDTO brandDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            try
            {                
                var brand = _mapper.Map<Brand>(brandDto);
                var check = await _repository.CheckBrandAsync(brand);
                if (!check)
                {
                    return StatusCode(406, new { message = "The brand name has been duplicated." });
                }   
                await _repository.CreateBrandAsync(brand);
                return Ok(new { message = "Brand added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] BrandDTO brandDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            try
            {                
                var brand = await _repository.GetBrandByIdAsync(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Brand is not found." });
                }

                _mapper.Map(brandDto, brand);
                var check = await _repository.CheckBrandAsync(brand);
                if (!check)
                {
                    return StatusCode(406, new { message = "The brand name has been duplicated." });
                }

                await _repository.UpdateBrandAsync(brand);
                return Ok(new { message = "Brand updated successfully." });
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
                var brand = await _repository.GetBrandByIdAsync(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return StatusCode(404,new { message = "Brand is not found." });
                }
                await _repository.DeleteBrandAsync(brand);
                return Ok(new { message = "Brand deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBrands(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return StatusCode(400,new { message = "Keyword must not be empty" });
                }

                var brands = await _repository.SearchBrandsAsync(keyword);
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }  
        }
    }
}
