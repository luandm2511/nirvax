using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<bool> AddCommentAsync(Comment comment);
        Task<bool> UpdateCommentAsync(Comment comment);
    }
}
