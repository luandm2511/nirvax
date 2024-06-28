using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class CommentDAO
    {
        private readonly NirvaxContext _context;

        public CommentDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments.Include(c => c.Product)
                .Include(c => c.Account)
                .Include(c => c.Owner)
                .Where(c => c.ProductId == productId).ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.Include(c => c.Product)
                    .Include(c => c.Account)
                    .Include(c => c.Owner)
                    .FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {
            comment.Timestamp = DateTime.Now;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
