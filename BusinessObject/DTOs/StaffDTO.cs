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
     
      
        public int StaffId { get; set; }

        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = " Password to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24  characters!!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Fullname is limited to 30 characters!!")]
        public string Fullname { get; set; } = null!;

        public string? Image { get; set; }
        
      //  public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
        [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Owner cannot be empty!!")]
        public int OwnerId { get; set; }
   
       

    }

    public class StaffCreateDTO
    {

        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = " Password to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24 characters!!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
        [MaxLength(30, ErrorMessage = "Fullname is limited to 30 characters!!")]
        public string Fullname { get; set; } = null!;

        public string? Image { get; set; }

        

        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
        [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Owner cannot be empty!!")]
        public int OwnerId { get; set; }



    }


    public class StaffAvatarDTO
    {

     
   
        public int StaffId { get; set; }
        public string? Image { get; set; }
        //[NotMapped]
       // public IFormFile? ImageFile { get; set; }
    }

    public class StaffProfileDTO
    {
       
        public int StaffId { get; set; }
        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
        [MaxLength(30, ErrorMessage = "Fullname is limited to 30 characters!!")]
        public string Fullname { get; set; } = null!;

        //public string? Image { get; set; }
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
        [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
        public string Phone { get; set; } = null!;

    }

}






