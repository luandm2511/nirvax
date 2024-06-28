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
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _accountDAO;

        public AccountRepository(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }

        public async Task<List<Account>> GetAllAccountAsync() => await _accountDAO.GetAllAccountAsync();
        public async Task<Account> GetAccountByIdAsync(int id) => await _accountDAO.GetAccountByIdAsync(id);
        public async Task<bool> BanAccountAsync(Account account) 
        {
            account.IsBan = true;
            return await _accountDAO.UpdateAccountAsync(account); 
        }
        public async Task<bool> UpdateAccountAsync(Account account) => await _accountDAO.UpdateAccountAsync(account);
        public async Task<IEnumerable<Account>> SearchAccountAsync(string keyword) => await _accountDAO.SearchAccountAsync(keyword);

        public async Task<bool> UnbanAccountAsync(Account account)
        {
            account.IsBan = false;
            return await _accountDAO.UpdateAccountAsync(account);
        }

    }
}
