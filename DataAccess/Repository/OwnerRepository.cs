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
    public class OwnerRepository : IOwnerRepository
    {
        private readonly OwnerDAO _owner;
        public OwnerRepository(OwnerDAO owner)
        {
            _owner = owner;
        }

        public async Task<IEnumerable<Owner>> SearchOwnersAsync(string? searchQuery)
        {
            return await _owner.SearchOwnersAsync(searchQuery);
        }
    }
}
