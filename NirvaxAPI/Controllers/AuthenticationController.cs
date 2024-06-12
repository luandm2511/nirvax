using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticationRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AuthenticationController(IConfiguration config, IAuthenticationRepository repo, IMemoryCache cache, IEmailService emailService)
        {       
            _config = config;
            _repository = repo;
            _emailService = emailService;
            _cache = cache;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingAccount = await _repository.CheckEmailAsync(request.Email);
            if (!existingAccount) return BadRequest("Email already in use.");

            var existingPhoneAccount = await _repository.CheckPhoneAsync(request.Phone);
            if (!existingPhoneAccount) return BadRequest("Phone number already in use.");

            var verificationCode = new Random().Next(100000, 999999).ToString();
            _cache.Set(request.Email, verificationCode, TimeSpan.FromMinutes(5));

            await _emailService.SendEmailAsync(request.Email, "Email Verification", $"Your verification code is: {verificationCode}");

            return Ok("Verification code sent. Please check your email.");
        }

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwner request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingAccount = await _repository.CheckEmailAsync(request.Email);
            if (existingAccount) return BadRequest("Email already in use.");

            var existingPhoneAccount = await _repository.CheckPhoneAsync(request.Phone);
            if (existingPhoneAccount) return BadRequest("Phone number already in use.");

            var verificationCode = new Random().Next(100000, 999999).ToString();
            _cache.Set(request.Email, verificationCode, TimeSpan.FromMinutes(5));

            await _emailService.SendEmailAsync(request.Email, "Email Verification", $"Your verification code is: {verificationCode}");

            return Ok("Verification code sent. Please check your email.");
        }

        [HttpPost("verify-user")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUser request)
        {
            if (_cache.TryGetValue(request.Email, out string storedCode))
            {
                if (storedCode == request.Code)
                {
                    _cache.Remove(request.Email);
                    var account = new Account
                    {
                        Email = request.Email,
                        Password = PasswordHasher.HashPassword(request.Password),
                        Fullname = request.Fullname,
                        Phone = request.Phone,
                        Dob = request.Dob,
                        Gender = request.Gender,
                        Address = request.Address,
                        Role = "User",
                        IsBan = false
                    };

                    await _repository.AddAccountAsync(account);
                    return Ok("Registration successful.");
                }
                else
                {
                    return BadRequest("Invalid verification code.");
                }
            }
            else
            {
                return BadRequest("Verification code expired.");
            }
        }

        [HttpPost("verify-owner")]
        public async Task<IActionResult> VerifyOwner([FromBody] VerifyOwner request)
        {
            if (_cache.TryGetValue(request.Email, out string storedCode))
            {
                if (storedCode == request.Code)
                {
                    _cache.Remove(request.Email);
                    var owner = new Owner
                    {
                        Email = request.Email,
                        Password = PasswordHasher.HashPassword(request.Password),
                        Fullname = request.Fullname,
                        Phone = request.Phone,
                        Address = request.Address,
                        IsBan = false
                    };

                    await _repository.AddOwnerAsync(owner);
                    return Ok("Registration successful.");
                }
                else
                {
                    return BadRequest("Invalid verification code.");
                }
            }
            else
            {
                return BadRequest("Verification code expired.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            // Check if email exists
            var account = await _repository.CheckEmailAsync(email);

            if (account)
            {
                return BadRequest("Email not found.");
            }

            var resetCode = new Random().Next(100000, 999999).ToString();
            _cache.Set(email, resetCode, TimeSpan.FromMinutes(2));

            await _emailService.SendEmailAsync(email, "Password Reset", $"Your password reset code is: {resetCode}");

            return Ok("Password reset code sent. Please check your email.");
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCode request)
        {
            if (_cache.TryGetValue(request.Email, out string storedCode))
            {
                if (storedCode == request.Code)
                {
                    _cache.Remove(request.Email);
                    return Ok("Verify code successful.");
                }
                else
                {
                    return BadRequest("Invalid reset code.");
                }
            }
            else
            {
                return BadRequest("Reset code expired.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        {
            var account = await _repository.GetAccountByEmailAsync(request.Email);
            if (account != null)
            {
                account.Password = PasswordHasher.HashPassword(request.NewPassword);
                await _repository.SaveChangesAsync();
                return Ok(new { message = "Reset password successful", userType = "User" });
            }

            var owner = await _repository.GetOwnerByEmailAsync(request.Email);
            if (owner != null)
            {
                owner.Password = PasswordHasher.HashPassword(request.NewPassword);
                await _repository.SaveChangesAsync();
                return Ok(new { message = "Reset password successful", userType = "Owner" });
            }

            var staff = await _repository.GetStaffByEmailAsync(request.Email);
            if (staff != null)
            {
                staff.Password = PasswordHasher.HashPassword(request.NewPassword);
                await _repository.SaveChangesAsync();
                return Ok(new { message = "Reset password successful", userType = "Staff" });
            }
            return BadRequest("Email not found");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            if (request.Role == "User" || request.Role == "Admin")
            {
                var account = await _repository.GetAccountByEmailAsync(request.Email);
                if (account != null && PasswordHasher.VerifyPassword(request.Password, account.Password))
                {
                    var token = GenerateJSONWebToken(account.Email, account.Role);
                    return Ok(new { token, userType = account.Role });
                }
            }
            else if (request.Role == "Shop")
            {
                var owner = await _repository.GetOwnerByEmailAsync(request.Email);
                if (owner != null && PasswordHasher.VerifyPassword(request.Password, owner.Password))
                {
                    var token = GenerateJSONWebToken(owner.Email, "Owner");
                    return Ok(new { token, userType = "Owner" });
                }

                var staff = await _repository.GetStaffByEmailAsync(request.Email);
                if (staff != null && PasswordHasher.VerifyPassword(request.Password, owner.Password))
                {
                    var token = GenerateJSONWebToken(staff.Email, "Staff");
                    return Ok(new { token, userType = "Staff" });
                }
            }
            return BadRequest("Invalid email or password.");
        }

        private string GenerateJSONWebToken(string Email, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
