﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task<bool> CheckPhoneAsync(int accountId, string phone);
        Task BanAccountAsync(Account account);
        Task UnbanAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task<IEnumerable<Account>> SearchAccountAsync(string keyword);
        Task<IEnumerable<AccountStatisticDTO>> GetAccountStatisticsAsync();
    }
}
