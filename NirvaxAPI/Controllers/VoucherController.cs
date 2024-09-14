using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _repo;
        private readonly string ok = "successfully ";
        private readonly string notFound = "Not found ";
        private readonly string badRequest = "Failed! ";

        public VoucherController(IVoucherRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> GetAllVouchersAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var list = await _repo.GetAllVouchersAsync(searchQuery, page, pageSize, ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list of vouchers " + ok,
                    Data = list
                });
            }
            else
            {
                return NoContent();

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVoucherForUserAsync()
        {
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
                return NoContent();

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVoucherByOwnerAsync(int ownerId)
        {
            var list = await _repo.GetAllVoucherByOwnerAsync(ownerId);
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
                return NoContent();

            }
        }


        [HttpGet("{voucherId}")]
        public async Task<ActionResult> GetVoucherByIdAsync(string voucherId)
        {
            var voucher = await _repo.GetVoucherDTOByIdAsync(voucherId);
            if (voucher != null)
            {
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


        [HttpPost]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> CreateVoucherAsync(VoucherCreateDTO voucherCreateDTO)
        {
            try
            {
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
                        Message = "Please enter valid Voucher!",
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
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> UpdateVoucherAsync(VoucherDTO voucherDTO)
        {
            try
            {
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
                    Message = "Please enter valid Voucher!",
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
        [Authorize(Roles = "Owner,Staff")]
        public async Task<ActionResult> DeleteVoucherAsync(string voucherId)
        {
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

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> ViewVoucherStatisticsAsync(int ownerId)
        {
            try
            {
                var total = await _repo.ViewVoucherStatisticsAsync(ownerId);
                return StatusCode(200, new
                {
                    Data = total
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }
    }
}
