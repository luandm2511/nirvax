using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class StaffDTO
    {  
        [Required(ErrorMessage = " StaffId cannot be empty!!")]
        public int StaffId { get; set; }

        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Email is limited to 100 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Password cannot be empty!!")]
        [MinLength(8, ErrorMessage = " Password to be at least 8 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24  characters!!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Fullname to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
        public string Fullname { get; set; } = null!;
        public string? Image { get; set; }
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be exactly 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }

    public class StaffCreateDTO
    {
        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Email is limited to 100 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Password cannot be empty!!")]
        [MinLength(8, ErrorMessage = " Password to be at least 8 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24 characters!!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Fullname to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
        public string Fullname { get; set; } = null!;
        public string? Image { get; set; }
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be exactly 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
    }


    public class StaffAvatarDTO
    {
        [Required(ErrorMessage = " StaffId cannot be empty!!")]
        public int StaffId { get; set; }
        public string Image { get; set; }
    }

    public class StaffProfileDTO
    {       
        [Required(ErrorMessage = " StaffId cannot be empty!!")]
        public int StaffId { get; set; }
        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Email is limited to 100 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Fullname to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
        public string Fullname { get; set; } = null!;
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be exactly 10 characters!!")]
        public string Phone { get; set; } = null!;
    }

}






