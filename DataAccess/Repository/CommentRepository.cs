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
    public class CommentRepository : ICommentRepository
    {
        public async Task<bool> AddCommentAsync(Comment comment) => await CommentDAO.AddCommentAsync(comment);

        public async Task<Comment> GetCommentByIdAsync(int commentId) => await CommentDAO.GetCommentByIdAsync(commentId);

        public async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId) => await CommentDAO.GetCommentsByProductIdAsync(productId);

        public async Task<bool> UpdateCommentAsync(Comment comment) => await CommentDAO.UpdateCommentAsync(comment);
    }
}
