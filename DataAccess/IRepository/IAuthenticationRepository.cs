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
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Owner> GetOwnerByEmailAsync(string email);
        Task<Staff> GetStaffByEmailAsync(string email);
        Task<bool> CheckPhoneAsync(string phone);
        Task<bool> AddAccountAsync(Account account);
        Task<bool> AddOwnerAsync(Owner owner);
        Task<bool> CheckEmailAsync(string email);
        Task<bool> SaveChangesAsync();
    }
}
