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
        private readonly CommentDAO _commentDAO;
        public CommentRepository(CommentDAO commentDAO)
        {
            _commentDAO = commentDAO;
        }

        public async Task<bool> AddCommentAsync(Comment comment) => await _commentDAO.AddCommentAsync(comment);

        public async Task<Comment> GetCommentByIdAsync(int commentId) => await _commentDAO.GetCommentByIdAsync(commentId);

        public async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId) => await _commentDAO.GetCommentsByProductIdAsync(productId);

        public async Task<bool> UpdateCommentAsync(Comment comment) => await _commentDAO.UpdateCommentAsync(comment);
    }
}
