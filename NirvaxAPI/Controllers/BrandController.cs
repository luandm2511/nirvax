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
        private readonly IMapper _mapper;

        public BrandController(IBrandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var brands = await _repository.GetAllBrandAsync();
                return Ok(brands);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
                    return StatusCode(404, new { message = "The brand has been not found." });
                }
                return Ok(brand);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
                    return StatusCode(404, new { message = "The brand has been not found." });
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
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
                    return StatusCode(404, new { message = "The brand has been not found." });
                }
                await _repository.DeleteBrandAsync(brand);
                return Ok(new { message = "Brand deleted successfully." });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBrands(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return StatusCode(400, new { message = "Keyword must not be empty" });
                }

                var brands = await _repository.SearchBrandsAsync(keyword);
                return Ok(brands);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }
    }
}
