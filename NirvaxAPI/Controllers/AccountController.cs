﻿using AutoMapper;
using Azure.Core;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountController(IAccountRepository repository, IMapper mapper, IEmailService emailService)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _repository.GetAllAccountAsync();
                return Ok(accounts);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
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
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPut("ban/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanAccount(int id) 
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                await _repository.BanAccountAsync(account);
                await _emailService.SendEmailAsync(account.Email, "Ban Account", "Your account violates the policy, so we temporarily and permanently block your account!");
                return Ok(new { message = "Account banned successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPut("unban/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnbanAccount(int id)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                await _repository.UnbanAccountAsync(account);
                await _emailService.SendEmailAsync(account.Email, "UnBan Account", "Through re-checking, we found that you have violated online trading standards but the impact is not too large, so we decided to re-unlock your account. Congratulations!!!");
                return Ok(new { message = "Account unbanned successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchAccounts([FromQuery] string keyword)
        {
            try
            {
                var accounts = await _repository.SearchAccountAsync(keyword);
                return Ok(accounts);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPut("{id}/change-password")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePassword changePassword)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                if(!PasswordHasher.VerifyPassword(changePassword.OldPassword, account.Password))
                {
                    return StatusCode(406, new { message = "The old password is incorrect." });
                }
                if (changePassword.NewPassword != changePassword.ConfirmPassword)
                {
                    return StatusCode(406, new { message = "The new password and confim password do not match." });
                }
                account.Password = PasswordHasher.HashPassword(changePassword.NewPassword);
                await _repository.UpdateAccountAsync(account);
                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPut("update-profile/{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAccount(int id,UpdateUserDTO model)
        {
            try
            {
                var check = await _repository.CheckPhoneAsync(id, model.Phone);
                if (!check)
                {
                    return StatusCode(406, new { message = "The phone number has been used!" });
                }
                var account = await _repository.GetAccountByIdAsync(id);
                _mapper.Map(model, account);
                await _repository.UpdateAccountAsync(account);
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPut("update-avatar/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateAvatar(int id, AvatarDTO avatarDTO)
        {
            try
            {
                var account = await _repository.GetAccountByIdAsync(id);
                account.Image = avatarDTO.Image;
                await _repository.UpdateAccountAsync(account);
                return Ok(new { message = "Avatar updated successfully." });
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpGet("account-statistics")]
        public async Task<IActionResult> AccountStatistics()
        {
            try
            {
                var statis = await _repository.GetAccountStatisticsAsync();
                return Ok(statis);
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }
    }

}
