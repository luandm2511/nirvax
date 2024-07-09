using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.DAOs
{
    public class AuthenticationDAO
    {
        private readonly NirvaxContext _context;

        public AuthenticationDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByEmailAsync(string email) => await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

        public async Task<Owner> GetOwnerByEmailAsync(string email) => await _context.Owners.FirstOrDefaultAsync(o => o.Email == email);

        public async Task<Staff> GetStaffByEmailAsync(string email) => await _context.Staff.FirstOrDefaultAsync(o => o.Email == email);

        public async Task<Account> GetAccountByPhoneAsync(string phone) => await _context.Accounts.FirstOrDefaultAsync(a => a.Phone == phone);

        public async Task<Owner> GetOwnerByPhoneAsync(string phone) => await _context.Owners.FirstOrDefaultAsync(o => o.Phone == phone);

        public async Task AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task AddOwnerAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();  
        }
        public async Task<bool> CheckPhoneAsync(string phone)
        {
            if (await _context.Accounts
                    .AnyAsync(p => p.Phone == phone)) return false;
            if (await _context.Owners
                    .AnyAsync(p => p.Phone == phone)) return false;
            if (await _context.Staff.AnyAsync(o => o.Phone == phone)) return false;
            return true;
        }
        public async Task<bool> CheckEmailAsync(string email)
        {
            if (await _context.Accounts
                    .AnyAsync(p => p.Email == email)) return false;
            if (await _context.Owners
                    .AnyAsync(p => p.Email == email)) return false;
            if (await _context.Staff.AnyAsync(p => p.Email == email)) return false;
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}