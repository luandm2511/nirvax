﻿using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public CommentController(IHubContext<NotificationHub> hubContext, IProductRepository productRepository, ICommentRepository commentRepository, INotificationRepository notificationRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _hubContext = hubContext;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _notificationRepository = notificationRepository;
            _transactionRepository = transactionRepository;
        }

        // Lấy tất cả comments của một sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetComments(int productId)
        {
            try
            {
                var comments = await _commentRepository.GetCommentsByProductIdAsync(productId);
                return Ok(comments);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong, please try again." });
            }
        }

        [HttpPost("add-comment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddComment([FromForm] CommentDTO commentDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { message = "Please pass the valid data." });
            }
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var product = await _productRepository.GetByIdAsync(commentDto.ProductId);
                if (product == null || product.Isdelete == true || product.Isban == true)
                {
                    return StatusCode(404, new { message = "The product has been deleted or banned. You can't rate and comment product" });
                }
                var comment = _mapper.Map<Comment>(commentDto);

                await _commentRepository.AddCommentAsync(comment);
                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = product.OwnerId, // Assuming Product model has OwnerId field
                    Content = $"Your product has just been commented.",
                    IsRead = false,
                    Url = null,
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"Owner-{product.OwnerId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok(new { message = "Comment is created successfully." });
            }
            catch (Exception)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }

        [HttpPut("reply/{commentId}")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> ReplyComment(int commentId, [FromBody] ReplyCommentDTO replyDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(404, new { message = "Please pass the valid data." });
            }
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var comment = await _commentRepository.GetCommentByIdAsync(commentId);

                var product = await _productRepository.GetByIdAsync(comment.ProductId);

                if (product == null || product.Isdelete == true || product.Isban == true)
                {
                    return StatusCode(404, new { message = "The product has not been found.." });
                }

                _mapper.Map(replyDto, comment);
                comment.ReplyTimestamp = DateTime.Now;

                await _commentRepository.UpdateCommentAsync(comment);
                var notification = new Notification
                {
                    AccountId = comment.AccountId,
                    OwnerId = null, // Assuming Product model has OwnerId field
                    Content = $"Your comment has just been replied.",
                    IsRead = false,
                    Url = "http://localhost:4200/product-detail/{productId}",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho người dùng
                await _hubContext.Clients.Group($"User-{comment.AccountId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok(new { message = "Reply successfully." });
            }
            catch (Exception)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again."
                });
            }
        }
    }
}
