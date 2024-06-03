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
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticationRepository _repo;

        public AuthenticationController(IConfiguration config, IAuthenticationRepository repo)
        {       
            _config = config;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async  Task<IActionResult> Login([FromBody] Login login)
        {
            if (login.Role.Equals("User"))
            {
                Account acc = _repo.LoginUser(login);
                if (acc != null)
                {
                    if(!_repo.CheckPW(acc.Password, login.Password))
                    {
                        return StatusCode(500, new
                        {
                            Status = "Login fail",
                            Message = "Password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Unauthorize",
                        Message = "Your account does not have permission to log in to this website."
                    });
                }
            }else if (login.Role.Equals("Admin"))
            {
                Account acc = _repo.LoginAdmin(login);
                if (acc != null)
                {
                    if (!_repo.CheckPW(acc.Password, login.Password))
                    {
                        return StatusCode(500, new
                        {
                            Status = "Login fail",
                            Message = "Email or password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Unauthorize",
                        Message = "Your account does not have permission to log in to this website."
                    });
                }
            } else if (login.Role.Equals("Shop"))
            {
                Owner owner = _repo.LoginShop(login);
                if (owner != null)
                {
                    if (!_repo.CheckPW(owner.Password, login.Password))
                    {
                        return StatusCode(500, new
                        {
                            Status = "Login fail",
                            Message = "Email or password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Unauthorize",
                        Message = "Your account does not have permission to log in to this website."
                    });
                }
            }
            else if (login.Role.Equals("Staff"))
            {
                Staff staff = _repo.LoginStaff(login);
                if (staff != null)
                {
                    if (!_repo.CheckPW(staff.Password, login.Password))
                    {
                        return StatusCode(500, new
                        {
                            Status = "Login fail",
                            Message = "Email or password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Status = "Unauthorize",
                        Message = "Your account does not have permission to log in to this website."
                    });
                }
            }
            var tokenString = GenerateJSONWebToken(login);
            return Ok(new { token = tokenString });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        private string GenerateJSONWebToken(Login userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString())
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
