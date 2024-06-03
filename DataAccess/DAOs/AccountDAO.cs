using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class AccountDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static async Task<List<Account>> GetAllAccountAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public static async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public static async Task<bool> UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<IEnumerable<Account>> SearchAccountAsync(string keyword)
        {
            return await _context.Accounts.Where(a => a.Fullname.Contains(keyword) || a.Email.Contains(keyword)).ToListAsync();
        }

        public static async Task<bool> ChangePasswordAsync(int id, string newPassword)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                account.Password = newPassword;
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
