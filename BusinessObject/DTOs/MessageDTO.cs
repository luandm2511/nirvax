using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class MessageDTO
    {
        [Required(ErrorMessage = " MessageId cannot be empty!!")]
        public int MessageId { get; set; }
        [Required(ErrorMessage = " SenderId cannot be empty!!")]
        public int SenderId { get; set; }
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        [Required(ErrorMessage = "SenderType cannot be empty!!")]
        public string SenderType { get; set; }
        [Required(ErrorMessage = " RoomId cannot be empty!!")]
        public int RoomId { get; set; }
    }

    public class MessageCreateDTO
    {
        [Required(ErrorMessage = " SenderId cannot be empty!!")]
        public int SenderId { get; set; }
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        [Required(ErrorMessage = "SenderType cannot be empty!!")]
        public string SenderType { get; set; }
        [Required(ErrorMessage = " RoomId cannot be empty!!")]
        public int RoomId { get; set; }
    }
}
