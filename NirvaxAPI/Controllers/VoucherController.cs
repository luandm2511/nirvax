﻿using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
            private readonly IConfiguration _config;
            private readonly IVoucherRepository  _repo;
            private readonly string ok = "successfully ";
            private readonly string notFound = "Not found ";
            private readonly string badRequest = "Failed! ";

            public VoucherController(IConfiguration config, IVoucherRepository repo)
            {
                _config = config;
                 _repo = repo;
            }


            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchersAsync(string? searchQuery, int page, int pageSize)
            {
                var list = await _repo.GetAllVouchersAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list voucher " + ok,
                        Data = list
                    });
                }
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any voucher"
                });
            }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVoucherForUserAsync()
        {
            var list = await _repo.GetAllVoucherForUserAsync();
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list voucher " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any voucher"
            });
        }


     
          




            [HttpGet("{voucherId}")]
            //  [Authorize]
            public async Task<ActionResult> GetProductSizeByIdAsync(string voucherId)
            {
                var checkVoucher = await _repo.CheckVoucherByIdAsync(voucherId);
                if (checkVoucher == true)
                {
                    var voucher = await _repo.GetVoucherByIdAsync(voucherId);
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get voucher by id " + ok,
                        Data = voucher
                    });
                }

                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any voucher"
                });
            }

            //check exist
            [HttpPost]
            public async Task<ActionResult> CreateVoucherAsync(VoucherDTO voucherDTO)
            {
            try { 
            if (ModelState.IsValid)
            {
                var checkVoucher = await _repo.CheckVoucherAsync(voucherDTO);
                if (checkVoucher == true)
                {
                    var voucher1 = await _repo.CreateVoucherAsync(voucherDTO);
                    if (voucher1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create voucher " + ok,
                            Data = voucher1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            Result = true,
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }

            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a voucher with that information",
                });
            }
        }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Please enter valid Staff",
            });
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
            public async Task<ActionResult> UpdateVoucherAsync(VoucherDTO voucherDTO)
            {
            try { 
            if (ModelState.IsValid)
            {
                var checkVoucher = await _repo.CheckVoucherExistAsync(voucherDTO);
                if (checkVoucher == true)
                {
                    var voucher1 = await _repo.UpdateVoucherAsync(voucherDTO);
                    if (voucher1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update voucher" + ok,
                            Data = voucher1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            Result = true,
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }
            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a voucher with that information",
                });
            }
        }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Please enter valid Staff",
            });
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




            [HttpPatch("{voucherId}")]
            public async Task<ActionResult> DeleteVoucherAsync(string voucherId)
            {
                var voucher1 = await _repo.DeleteVoucherAsync(voucherId);
                if (voucher1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Delete voucher " + ok,

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
        public async Task<ActionResult> QuantityVoucherUsedStatisticsAsync(int ownerId)
        {
            try
            {
                var number = await _repo.QuantityVoucherUsedStatisticsAsync(ownerId);
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> PriceVoucherUsedStatisticsAsync(int ownerId)
        {
            try { 
            var number = await _repo.PriceVoucherUsedStatisticsAsync(ownerId);
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> PriceAndQuantityByOrderAsync(string voucherId, int quantity)
        {
            try {
                var result = await _repo.PriceAndQuantityByOrderAsync( voucherId, quantity);
                if (result == true)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update voucher after used success!",

                    });
                }
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = badRequest,
                });
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