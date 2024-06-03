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
        Task<List<Account>> GetAllAccountAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task<bool> BanAccountAsync(Account account);
        Task<bool> UnbanAccountAsync(Account account);
        Task<bool> UpdateAccountAsync(Account account);
        Task<IEnumerable<Account>> SearchAccountAsync(string keyword);
        Task<bool> ChangePasswordAsync(int id, string newPassword);
    }
}
