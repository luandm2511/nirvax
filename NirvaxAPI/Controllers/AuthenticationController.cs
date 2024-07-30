using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
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
        private readonly IMapper _mapper;

        public AuthenticationController(IConfiguration config, IAuthenticationRepository repo, IMemoryCache cache, IEmailService emailService, IMapper mapper)
        {       
            _config = config;
            _repository = repo;
            _emailService = emailService;
            _cache = cache;
            _mapper = mapper;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            try
            {
                var existingAccount = await _repository.CheckEmailAsync(request.Email);
                if (!existingAccount) return StatusCode(400, new { message = "Email already in use." });

                var existingPhoneAccount = await _repository.CheckPhoneAsync(request.Phone);
                if (!existingPhoneAccount) return StatusCode(400, new { message = "Phone number already in use." });

                if (request.Password != request.ConfirmPassword)
                {
                    return StatusCode(400, new { message = "Passwords do not match." });
                }

                var verificationCode = new Random().Next(100000, 999999).ToString();
                _cache.Set(request.Email, verificationCode, TimeSpan.FromMinutes(5));

                await _emailService.SendEmailAsync(request.Email, "Email Verification", $"Your verification code is: {verificationCode}");

                return Ok("Verification code sent. Please check your email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }         
        }

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwner request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            try
            {
                var existingAccount = await _repository.CheckEmailAsync(request.Email);
                if (!existingAccount) return StatusCode(400, new { message = "Email already in use." });

                var existingPhoneAccount = await _repository.CheckPhoneAsync(request.Phone);
                if (!existingPhoneAccount) return StatusCode(400, new { message = "Phone number already in use." });

                if (request.Password != request.ConfirmPassword)
                {
                    return StatusCode(400, new { message = "Passwords do not match." });
                }

                var verificationCode = new Random().Next(100000, 999999).ToString();
                _cache.Set(request.Email, verificationCode, TimeSpan.FromMinutes(5));

                await _emailService.SendEmailAsync(request.Email, "Email Verification", $"Your verification code is: {verificationCode}");

                return Ok("Verification code sent. Please check your email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("verify-user")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUser request)
        {
            try 
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
                        return StatusCode(400, new { message = "Invalid verification code." });
                    }
                }
                else
                {
                    return StatusCode(400, new { message = "Verification code expired." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("verify-owner")]
        public async Task<IActionResult> VerifyOwner([FromBody] VerifyOwner request)
        {
            try
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
                        var warehouse = new Warehouse
                        {
                            OwnerId = owner.OwnerId,
                            TotalQuantity = 0,
                            TotalPrice = 0.0
                        };
                        await _repository.AddWarehouseAsync(warehouse);
                        return Ok("Registration successful.");
                    }
                    else
                    {
                        return StatusCode(400, new { message = "Invalid verification code." });
                    }
                }
                else
                {
                    return StatusCode(400, new { message = "Verification code expired." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }         
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                // Check if email exists
                var account = await _repository.CheckEmailAsync(email);

                if (account)
                {
                    return StatusCode(404, new { message = "Email not found." });
                }

                var resetCode = new Random().Next(100000, 999999).ToString();
                _cache.Set(email, resetCode, TimeSpan.FromMinutes(5));

                await _emailService.SendEmailAsync(email, "Password Reset", $"Your password reset code is: {resetCode}");

                return Ok("Password reset code sent. Please check your email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }       
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCode request)
        {
            try
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
                        return StatusCode(400, new { message = "Invalid reset code." });
                    }
                }
                else
                {
                    return StatusCode(400, new { message = "Reset code expired." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }          
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return StatusCode(400, new { message = "Passwords do not match." });
            }
            try
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
                return StatusCode(404, new { message = "Email not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }           
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromForm] Login request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(406, new { message = "Please pass the valid data." });
                }
                var account = await _repository.GetAccountByEmailAsync(request.Email);
                if (account.Role == "Admin" && PasswordHasher.VerifyPassword(request.Password, account.Password))
                {
                    var token = GenerateJSONWebToken(account.AccountId, account.Email, account.Role);
                    return Ok(new { token, userType = account.Role });
                }
                return StatusCode(400, new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromForm] Login request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(406, new { message = "Please pass the valid data." });
            }
            try
            {
                var account = await _repository.GetAccountByEmailAsync(request.Email);
                if (account.IsBan)
                {
                    return StatusCode(400, new { message = "Your account is banned." });
                }

                if (account.Role == "User" && PasswordHasher.VerifyPassword(request.Password, account.Password))
                {
                    var token = GenerateJSONWebToken(account.AccountId,account.Email, account.Role);
                    return Ok(new { token, userType = account.Role });
                }
                return StatusCode(400, new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login-shop")]
        public async Task<IActionResult> LoginShop([FromForm] Login request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(406, new { message = "Please pass the valid data." });
            }
            try
            {
                if(request.Role == "Owner")
                {
                    var owner = await _repository.GetOwnerByEmailAsync(request.Email);
                    if (owner.IsBan)
                    {
                        return StatusCode(400, new { message = "Your account is banned." });
                    }
                    if (owner != null && PasswordHasher.VerifyPassword(request.Password, owner.Password))
                    {
                        var token = GenerateJSONWebToken(owner.OwnerId,owner.Email, "Owner");
                        return Ok(new { token, userType = "Owner" });
                    }
                }               
                if (request.Role == "Staff")
                {
                    var staff = await _repository.GetStaffByEmailAsync(request.Email);
                    if (staff != null && PasswordHasher.VerifyPassword(request.Password, staff.Password))
                    {
                        var token = GenerateJSONWebToken(staff.StaffId, staff.Email, "Staff");
                        return Ok(new { token, userType = "Staff" });
                    }
                }               
                return StatusCode(400, new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync();
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var picture = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;
            var phoneNumber = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
            var address = claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value;

            Account user = await _repository.GetAccountByEmailAsync(email);

            if (user == null)
            {
                var userGG = new AccountGoogle
                {
                    Email = email,
                    Fullname = name,
                    Phone = phoneNumber,
                    Image = picture,
                    Address = address,
                    Role = "User",
                    IsBan = false
                };
                var account = _mapper.Map<Account>(userGG);
                await _repository.AddAccountAsync(account);
                var tokenn = GenerateJSONWebToken(account.AccountId, email, "User");
                return Ok(new { token = tokenn });
            }

            var tokenString = GenerateJSONWebToken(user.AccountId, email, "User");

            return Ok(new { token = tokenString });
        }

        private string GenerateJSONWebToken(int id, string Email, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
