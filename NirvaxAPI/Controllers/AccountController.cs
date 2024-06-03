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
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _repository.GetAllAccount();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAccountById(int id)
        {
            try
            {
                var account = _repository.GetAccountById(id);
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

        [HttpPost("{id}/ban")]
        public IActionResult BanAccount(int id)
        {
            try
            {
                var account = _repository.GetAccountById(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                _repository.BanAccount(account);
                return Ok(new { message = "Account banned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public IActionResult SearchAccounts([FromQuery] string keyword)
        {
            try
            {
                var accounts = _repository.SearchAccount(keyword);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/change-password")]
        public IActionResult ChangePassword(int id, [FromBody] string newPassword)
        {
            try
            {
                var account = _repository.GetAccountById(id);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                _repository.ChangePassword(id, newPassword);
                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/update-profile")]
        public IActionResult UpdateAccount(UpdateUserDTO model)
        {
            try
            {
                var account = _repository.GetAccountById(model.AccountId);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                var pro = _mapper.Map<Account>(model);
                _repository.UpdateAccount(pro);
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }

}
