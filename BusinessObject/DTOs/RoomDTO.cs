using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]

        public int OwnerId { get; set; }
        [Required(ErrorMessage = " AccountId cannot be empty!!")]

        public int AccountId { get; set; }
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
        [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
        public string Content { get; set; } = null!;

        public DateTime Timestamp { get; set; }
    }

    public class RoomCreateDTO
    {
        
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]

        public int OwnerId { get; set; }
        [Required(ErrorMessage = " AccountId cannot be empty!!")]

        public int AccountId { get; set; }
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
        [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
        public string Content { get; set; } = null!;

        public DateTime Timestamp { get; set; }
    }

    public class RoomContentDTO
    {
        public int RoomId { get; set; }
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]

        public string Content { get; set; } = null!;

        
    }
}
