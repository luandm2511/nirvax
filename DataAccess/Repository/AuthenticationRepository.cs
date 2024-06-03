using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{ 
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public Account LoginUser(Login login) =>  AuthenticationDAO.LoginUser(login); 
        public Account LoginAdmin(Login login) => AuthenticationDAO.LoginAdmin(login);
        public Owner LoginShop(Login login) => AuthenticationDAO.LoginShop(login);
        public Staff LoginStaff(Login login) => AuthenticationDAO.LoginStaff(login);
        public bool CheckPW(string Pw, string Password) => AuthenticationDAO.CheckPW(Pw, Password);
    }
}
