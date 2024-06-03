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
        public List<Account> GetAllAccount() => AccountDAO.GetAllAccount();
        public Account GetAccountById(int id) => AccountDAO.GetAccountById(id);

        public bool BanAccount(Account account) => AccountDAO.BanAccount(account);

        public bool UpdateAccount(Account account) => AccountDAO.UpdateAccount(account);

        public IEnumerable<Account> SearchAccount(string keyword) => AccountDAO.SearchAccount(keyword);

        public bool ChangePassword(int id, string newPassword) => AccountDAO.ChangePassword(id, newPassword);
    }
}
