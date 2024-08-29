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

        public async Task<AccountStatisticDTO> GetAccountStatisticsAsync()
        {
            var accounts = await _context.Accounts.Where(a => a.Role.Equals("User")).ToListAsync();
            var owners = await _context.Owners.ToListAsync();

            int totalAccounts = accounts.Count;
            int totalOwners = owners.Count;

            var accountStats = accounts
            .GroupBy(account => new
            {
                Year = account.CreatedDate.Year,
                StartDate = GetStartOfWeek(account.CreatedDate),
                EndDate = GetEndOfWeek(account.CreatedDate),
                DayOfWeek = (int)account.CreatedDate.DayOfWeek == 0 ? 7 : (int)account.CreatedDate.DayOfWeek
            })
            .Select(group => new
            {
                Year = group.Key.Year,
                StartDate = group.Key.StartDate,
                EndDate = group.Key.EndDate,
                DayOfWeek = group.Key.DayOfWeek,
                NumberAccounts = group.Count()
            })
            .ToList();

            var ownerStats = owners
                .GroupBy(owner => new
                {
                    Year = owner.CreatedDate.Year,
                    StartDate = GetStartOfWeek(owner.CreatedDate),
                    EndDate = GetEndOfWeek(owner.CreatedDate),
                    DayOfWeek = (int)owner.CreatedDate.DayOfWeek == 0 ? 7 : (int)owner.CreatedDate.DayOfWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    StartDate = group.Key.StartDate,
                    EndDate = group.Key.EndDate,
                    DayOfWeek = group.Key.DayOfWeek,
                    NumberOwners = group.Count()
                })
                .ToList();

            var combinedStats = accountStats
                .GroupJoin(ownerStats, acc => new { acc.Year, acc.StartDate, acc.EndDate, acc.DayOfWeek },
                                        own => new { own.Year, own.StartDate, own.EndDate, own.DayOfWeek },
                                        (acc, ownGroup) => new
                                        {
                                            acc.Year,
                                            acc.StartDate,
                                            acc.EndDate,
                                            acc.DayOfWeek,
                                            acc.NumberAccounts,
                                            NumberOwners = ownGroup.Sum(o => o.NumberOwners)
                                        })
                .ToList();

            var result = combinedStats
                .GroupBy(stat => new { stat.Year, stat.StartDate, stat.EndDate })
                .Select(weekGroup => new AccountStatisticByTime
                {
                    Year = weekGroup.Key.Year,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    DailyStatistics = weekGroup
                        .OrderBy(stat => stat.DayOfWeek)
                        .Select(stat => new DailyAccountStatistics
                        {
                            DayOfWeek = stat.DayOfWeek,
                            NumberAccounts = stat.NumberAccounts,
                            NumberOwners = stat.NumberOwners
                        })
                        .ToList()
                })
                .ToList();

            var statisticsResult = new AccountStatisticDTO
            {
                TotalAccountUser = totalAccounts,
                TotalAccountOwner = totalOwners,
                AccountStatistics = result
            };

            return statisticsResult;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
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
