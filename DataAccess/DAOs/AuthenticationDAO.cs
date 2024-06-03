using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.DAOs
{
    public class AuthenticationDAO
    {
        private readonly IConfiguration _config;
        public AuthenticationDAO(IConfiguration config)
        {
            _config = config;
        }

        public static Account LoginUser(Login login)
        {
            Account account;
            try
            {
                using (var _context = new NirvaxContext())
                {
                    account = _context.Accounts.FirstOrDefault(u => u.Email == login.Email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



            if (account == null || account.Role != "User")
            {
                return null;
            }
            return account;
        }

        public static Account LoginAdmin(Login login)
        {
            Account account;
            try
            {
                using (var _context = new NirvaxContext())
                {
                    account = _context.Accounts.FirstOrDefault(u => u.Email == login.Email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (account == null || account.Role != "Admin")
            {
                return null;
            }

            return account;
        }

        public static Owner LoginShop(Login login)
        {
            Owner owner;
            try
            {
                using (var _context = new NirvaxContext())
                {
                    owner = _context.Owners.FirstOrDefault(u => u.Email == login.Email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        public static Staff LoginStaff(Login login)
        {
            Staff staff;
            try
            {
                using (var _context = new NirvaxContext())
                {
                    staff = _context.Staff.FirstOrDefault(u => u.Email == login.Email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (staff == null)
            {
                return null;
            }
            return staff;
        }

        public static bool CheckPW(string pw, string password)
        {
            if (pw.Equals(password))
            {
                return true;
            }
            return false;   
        }

        
    }
}