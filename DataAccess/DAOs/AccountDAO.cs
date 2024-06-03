using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace DataAccess.DAOs
{
    public class AccountDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static List<Account> GetAllAccount()
        {
            try
            {
                return _context.Accounts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Account GetAccountById(int id)
        {
            try
            {
                return _context.Accounts.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool BanAccount(Account account)
        {
            try
            {
                account.IsBan = true;
                _context.Entry<Account>(account).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static bool UpdateAccount(Account account)
        {
            try
            {
                 _context.Entry<Account>(account).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                 _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static IEnumerable<Account> SearchAccount(string keyword)
        {
            try
            {
                return  _context.Accounts.Where(a => a.Fullname.Contains(keyword) || a.Email.Contains(keyword)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool ChangePassword(int id, string newPassword) 
        {
            try
            {
                var account = _context.Accounts.Find(id);
                if (account != null)
                {
                    account.Password = newPassword;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
