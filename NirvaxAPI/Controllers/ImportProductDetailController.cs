﻿using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ImportProductDetailController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IImportProductDetailRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public ImportProductDetailController(IConfiguration config, IImportProductDetailRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProductDetail>>> GetAllImportProductDetail()
        {
            var list = await _repo.GetAllImportProductDetail();
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list Import Product Detail " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Import Product Detail"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<ImportProductDetail>>> GetAllImportProductDetailByImportId(int importId)
        {
            var list = await _repo.GetAllImportProductDetailByImportId(importId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list Import Product Detail " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Import Product Detail"
            });
        }

        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateImportProductDetail(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {
            try
            {
                var importProductDetail = await _repo.CreateImportProductDetail(importId, importProductDetailDTO);

                if (importProductDetail == true)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Create Import Product Detail " + ok,
                        Data = importProductDetail
                    });
                }

                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Find fail",
                        Message = notFound + "any Import Product Detail"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpPut]
        //  [Authorize]
        public async Task<ActionResult> UpdateImportProductDetail(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {
            try
            {
                var importProductDetail = await _repo.UpdateImportProductDetail(importId, importProductDetailDTO);

                if (importProductDetail == true)
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Update Import Product Detail " + ok,
                        Data = importProductDetail
                    });
                }

                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Find fail",
                        Message = notFound + "any Import Product Detail"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


    }
}