﻿using AutoMapper;
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
                    return NotFound(new { message = "Brand not found." });
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }
                var brand = _mapper.Map<Brand>(brandDto);
                var check = await _repository.CheckBrandAsync(brand);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The brand name has been duplicated." });
                }   
                var result = await _repository.CreateBrandAsync(brand);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] BrandDTO brandDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(0, new { message = "Please pass the valid data." });
                }

                var brand = await _repository.GetBrandByIdAsync(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return NotFound(new { message = "Brand not found." });
                }

                _mapper.Map(brandDto, brand);
                var check = await _repository.CheckBrandAsync(brand);
                if (!check)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "The brand name has been duplicated." });
                }

                var result = await _repository.UpdateBrandAsync(brand);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await _repository.GetBrandByIdAsync(id);
                if (brand == null || brand.Isdelete == true)
                {
                    return NotFound(new { message = "Brand not found." });
                }
                var result = await _repository.DeleteBrandAsync(brand);
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
