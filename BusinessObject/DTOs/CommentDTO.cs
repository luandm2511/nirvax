using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class CommentDTO
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = " Comment cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Comment to be at least 2 characters!!")]
        [MaxLength(200, ErrorMessage = "Comment is limited to 200 characters!!")]
        public string Content { get; set; } = null!;
    }
}
