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
        public Task<bool> AddCommentAsync(Comment comment) => CommentDAO.AddCommentAsync(comment);

        public Task<Comment> GetCommentByIdAsync(int commentId) => CommentDAO.GetCommentByIdAsync(commentId);

        public Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId) => CommentDAO.GetCommentsByProductIdAsync(productId);

        public Task<bool> UpdateCommentAsync(Comment comment) => CommentDAO.UpdateCommentAsync(comment);
    }
}
