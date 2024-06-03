using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public AccountController(IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _repository.GetAllAccountAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/ban")]
        public async Task<IActionResult> BanAccount(int id)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                await _repository.BanAccountAsync(account);
                return Ok(new { message = "Account banned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/unban")]
        public async Task<IActionResult> UnbanAccount(int id)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                await _repository.UnbanAccountAsync(account);
                return Ok(new { message = "Account unbanned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAccounts([FromQuery] string keyword)
        {
            try
            {
                var accounts = await _repository.SearchAccountAsync(keyword);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] string newPassword)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                await _repository.ChangePasswordAsync(id, newPassword);
                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/update-profile")]
        public async Task<IActionResult> UpdateAccount(int id,UpdateUserDTO model)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                _mapper.Map(model, account);
                await _repository.UpdateAccountAsync(account);
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/{avatar}")]
        public async Task<IActionResult> UpdateAvatar(int id, string avatar)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                account.Image = avatar;
                await _repository.UpdateAccountAsync(account);
                return Ok(new { message = "Avatar updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }

}
