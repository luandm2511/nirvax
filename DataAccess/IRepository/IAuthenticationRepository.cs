using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IAuthenticationRepository
    {
        Account LoginUser(Login login);
        Account LoginAdmin(Login login);
        Owner LoginShop(Login login);
        Staff LoginStaff(Login login);
        bool CheckPW(string Pw, string Password);
    }
}
