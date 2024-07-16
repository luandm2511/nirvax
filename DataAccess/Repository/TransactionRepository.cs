using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDAO _dao;
        public TransactionRepository(TransactionDAO dao) 
        {
            _dao = dao;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dao.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
           await _dao.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dao.RollbackTransactionAsync();
        }
    }
}
