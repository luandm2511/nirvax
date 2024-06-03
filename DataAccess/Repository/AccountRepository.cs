using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<List<Account>> GetAllAccountAsync() => await AccountDAO.GetAllAccountAsync();
        public async Task<Account> GetAccountByIdAsync(int id) => await AccountDAO.GetAccountByIdAsync(id);
        public async Task<bool> BanAccountAsync(Account account) 
        {
            account.IsBan = true;
            return await AccountDAO.UpdateAccountAsync(account); 
        }
        public async Task<bool> UpdateAccountAsync(Account account) => await AccountDAO.UpdateAccountAsync(account);
        public async Task<IEnumerable<Account>> SearchAccountAsync(string keyword) => await AccountDAO.SearchAccountAsync(keyword);
        public async Task<bool> ChangePasswordAsync(int id, string newPassword) => await AccountDAO.ChangePasswordAsync(id, newPassword);

        public async Task<bool> UnbanAccountAsync(Account account)
        {
            account.IsBan = false;
            return await AccountDAO.UpdateAccountAsync(account);
        }
    }
}
