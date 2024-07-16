using BusinessObject.DTOs;
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
            try { 
                var list = await _repo.GetAllVouchersAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list voucher " + ok,
                    Data = list
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    Message = notFound + "any voucher"
                });
            }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVoucherForUserAsync()
        {
            try { 
            var list = await _repo.GetAllVoucherForUserAsync();
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list voucher " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any voucher"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


     
          




            [HttpGet("{voucherId}")]
            //  [Authorize]
            public async Task<ActionResult> GetProductSizeByIdAsync(string voucherId)
            {
            try { 
                var checkVoucher = await _repo.CheckVoucherByIdAsync(voucherId);
                if (checkVoucher == true)
                {
                    var voucher = await _repo.GetVoucherDTOByIdAsync(voucherId);
                    return StatusCode(200, new
                    {
                        Message = "Get voucher by id " + ok,
                        Data = voucher
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any voucher"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

            //check exist
            [HttpPost]
            public async Task<ActionResult> CreateVoucherAsync(VoucherCreateDTO voucherCreateDTO)
            {
            try {
                if (ModelState.IsValid)
                {
                    var checkVoucher = await _repo.CheckVoucherAsync(voucherCreateDTO.StartDate, voucherCreateDTO.EndDate, voucherCreateDTO.VoucherId);
                    if (checkVoucher == true)
                    {
                        var voucher1 = await _repo.CreateVoucherAsync(voucherCreateDTO);
                        return StatusCode(200, new
                        {
                            Message = "Create voucher " + ok,
                            Data = voucher1
                        });
                    }

                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a voucher with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Voucher",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
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
                        return StatusCode(200, new
                        {  
                            Message = "Update voucher" + ok,
                            Data = voucher1
                        });                
                }
            else
            {
                return StatusCode(400, new
                {
                    Message = "There already exists a voucher with that information",
                });
            }
        }
            return StatusCode(400, new
            {
                Message = "Please enter valid Voucher",
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }




            [HttpPatch("{voucherId}")]
            public async Task<ActionResult> DeleteVoucherAsync(string voucherId)
            {
            try { 
                var voucher1 = await _repo.DeleteVoucherAsync(voucherId);
                if (voucher1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Delete voucher " + ok,
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> VoucherStatisticsAsync(int ownerId)
        {
                var total = await _repo.QuantityVoucherUsedStatisticsAsync(ownerId);
                var total2 = await _repo.PriceVoucherUsedStatisticsAsync(ownerId);          
                return StatusCode(200, new
                    {
                        totalQuantityVoucherUsed = total,
                    totalPriceVoucherUsed = total2
                });                  
        }


        [HttpGet]
        public async Task<ActionResult> QuantityVoucherUsedStatisticsAsync(int ownerId)
        {
                var number = await _repo.QuantityVoucherUsedStatisticsAsync(ownerId);
                if (number != null)
                {
                    return StatusCode(200, new
                    {
                        Message = number,
                    });
                }
                return StatusCode(200, new
                { 
                    Message = 0,
                });         
        }

        [HttpGet]
        public async Task<ActionResult> PriceVoucherUsedStatisticsAsync(int ownerId)
        {
            var number = await _repo.PriceVoucherUsedStatisticsAsync(ownerId);
                if (number != null)
                {
                    return StatusCode(200, new
                    {
                        Message = number,
                    });
                }
                else
                {
                    return StatusCode(200, new
                    {
                        Message = 0,
                    });
                }         
        }

    }
}
