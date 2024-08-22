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
    public class AccountDAO
    {
        private readonly NirvaxContext _context;

        public AccountDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAccountAsync()
        {
            return await _context.Accounts.Where( a => !a.Role.Equals("Admin")).ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Account>> SearchAccountAsync(string keyword)
        {
            return await _context.Accounts.Where(a => a.Fullname.Contains(keyword) || a.Email.Contains(keyword)).ToListAsync();
        }

        public async Task<AccountStatisticDTO> AccountStatistics()
        {
            var totalAccount = await _context.Accounts.CountAsync();
            var totalAccountBanned = await _context.Accounts.CountAsync(a => a.IsBan);
            var totalOwner = await _context.Owners.CountAsync();
            var totalOwnerBanned = await _context.Owners.CountAsync(o => o.IsBan);
            return new AccountStatisticDTO
            {
                TotalAccount = totalAccount,
                TotalAccountBanned = totalAccountBanned,
                TotalOwner = totalOwner,
                TotalOwnerBanned = totalOwnerBanned
            };
        }

        public async Task<bool> CheckPhoneAsync(int accountId, string phone)
        {
            if (await _context.Accounts
                    .AnyAsync(p => p.Phone == phone && p.AccountId != accountId)) return false;
            if (await _context.Owners
                    .AnyAsync(p => p.Phone == phone)) return false;
            if (await _context.Staff.AnyAsync(o => o.Phone == phone)) return false;
            return true;
        }
    }
}
