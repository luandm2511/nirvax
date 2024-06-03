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
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            try
            {
                return await _context.Comments.Include(c => c.Product)
                    .Include(c => c.Account)
                    .Include(c => c.Owner)
                    .Where(c => c.ProductId == productId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the comment with  product ID {productId}.", ex);
            }
        }

        public static async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            try
            {
                return await _context.Comments.Include(c => c.Product)
                    .Include(c => c.Account)
                    .Include(c => c.Owner)
                    .FirstOrDefaultAsync(c => c.CommentId == commentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the comment with ID {commentId}.", ex);
            }
        }

        public static async Task<bool> AddCommentAsync(Comment comment)
        {
            try
            {
                comment.Timestamp = DateTime.Now;
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when adding comment.", ex);
            }
        }

        public static async Task<bool> UpdateCommentAsync(Comment comment)
        {
            try
            {
                 _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating comment.", ex);
            }
        }
    }
}
