using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ReplyCommentDTO
    {
        [Required(ErrorMessage = " Comment cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Comment to be at least 2 characters!!")]
        [MaxLength(200, ErrorMessage = "Comment is limited to 200 characters!!")]
        public string Reply { get; set; }

        [Required(ErrorMessage = " Owner cannot be empty!!")]
        public int OwnerId { get; set; }
        [Required(ErrorMessage = " ReplyTimestamp cannot be empty!!")]
        public DateTime ReplyTimestamp { get; set; }
    }
}
