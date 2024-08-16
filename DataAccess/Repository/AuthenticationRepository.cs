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
        private readonly AuthenticationDAO _authen;

        public AuthenticationRepository(AuthenticationDAO authen)
        {
            _authen = authen;
        }
        async Task IAuthenticationRepository.AddAccountAsync(Account account)
        {
            await _authen.AddAccountAsync(account);
        }

        async Task IAuthenticationRepository.AddOwnerAsync(Owner owner)
        {
            await _authen.AddOwnerAsync(owner);
        }

        async Task<Account> IAuthenticationRepository.GetAccountByEmailAsync(string email)
        {
            return await _authen.GetAccountByEmailAsync(email);
        }

        async Task<Owner> IAuthenticationRepository.GetOwnerByEmailAsync(string email)
        {
            return await _authen.GetOwnerByEmailAsync(email);
        }

        async Task<bool> IAuthenticationRepository.CheckPhoneAsync(string phone)
        {
            return await _authen.CheckPhoneAsync(phone);
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _authen.CheckEmailAsync(email);
        }

        public async Task<Staff> GetStaffByEmailAsync(string email)
        {
            return await _authen.GetStaffByEmailAsync(email);
        }
        public async Task SaveChangesAsync()
        {
            await _authen.SaveChangesAsync();
        }
    }
}
 
