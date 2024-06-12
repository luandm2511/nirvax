﻿using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class WarehouseDetailController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWarehouseDetailRepository _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public WarehouseDetailController(IConfiguration config, IWarehouseDetailRepository repo)
        {
            _config = config;
            _repo = repo;
        }



        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<WarehouseDetailFinalDTO>>> GetAllWarehouseDetailByProductSize(int warehouseId, int page, int pageSize)
        {
            var list = await _repo.GetAllWarehouseDetailByProductSize(warehouseId, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list Warehouse detail " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Warehouse detail"
            });
        }






        [HttpPost]
        //  [Authorize]
        public async Task<ActionResult> CreateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            var warehouse = await _repo.CreateWarehouseDetail(warehouseDetailDTO);

            if (warehouse == true)
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Create Warehouse detail" + ok,
                    Data = warehouse
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any Warehouse detail"
            });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            var size1 = await _repo.UpdateWarehouseDetail(warehouseDetailDTO);
            if (size1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Update Warehouse detail" + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> SumOfKindProdSizeStatistics(int warehouseId)
        {
            var number = await _repo.SumOfKindProdSizeStatistics(warehouseId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }


    }
}
