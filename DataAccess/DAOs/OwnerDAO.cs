using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class OwnerDAO
    {
        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;

        public OwnerDAO(NirvaxContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Owner>> SearchOwnersAsync(string? searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return await _context.Owners
                   .Where(i => i.Fullname.Contains(searchQuery))
                   .Where(i => i.IsBan == false)
                   .ToListAsync();
            }
            return null;
        }
    }
}
