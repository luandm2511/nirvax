using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IAccountRepository
    {
        List<Account> GetAllAccount();
        Account GetAccountById(int id);
        bool BanAccount(Account account);
        bool UpdateAccount(Account account);
        IEnumerable<Account> SearchAccount(string keyword);
        bool ChangePassword(int id, string newPassword);
    }
}
