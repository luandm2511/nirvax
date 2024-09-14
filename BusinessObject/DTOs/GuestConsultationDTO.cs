using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs
{
    public class GuestConsultationDTO
    {
        public int GuestId { get; set; }
        [Required(ErrorMessage = " FullName cannot be empty!!")]
        [MinLength(1, ErrorMessage = " FullName to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = " FullName to be at least 50 characters!!")]
        public string Fullname { get; set; } = null!;
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(8, ErrorMessage = " Phone to be at least 8 characters!!")]
        [MaxLength(10, ErrorMessage = " Phone to be at least 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(500, ErrorMessage = " Content to be at least 500 characters!!")]
        public string Content { get; set; } = null!;
        [Required(ErrorMessage = "StatusGuestId cannot be empty!!")]
        public int StatusGuestId { get; set; }
        [Required(ErrorMessage = "AdId cannot be empty!!")]
        public int AdId { get; set; }
        [Required(ErrorMessage = "OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }

    public class GuestConsultationCreateDTO
    {
        [Required(ErrorMessage = " FullName cannot be empty!!")]
        [MinLength(1, ErrorMessage = " FullName to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = " FullName to be at least 50 characters!!")]
        public string Fullname { get; set; } = null!;
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(8, ErrorMessage = " Phone to be at least 8 characters!!")]
        [MaxLength(10, ErrorMessage = " Phone to be at least 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Content cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Content to be at least 1 characters!!")]
        [MaxLength(500, ErrorMessage = " Content to be at least 500 characters!!")]
        public string Content { get; set; } = null!;

         [Required(ErrorMessage = "AdId cannot be empty!!")]
        public int AdId { get; set; }
        [Required(ErrorMessage = "OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }
}
