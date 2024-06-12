using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.DTOs {

    public class OwnerDTO
    {
        public int OwnerId { get; set; }
        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = " Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = " Password to be at least 6 characters!!")]
        [MaxLength(10, ErrorMessage = "Password is limited to 10 characters!!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(5, ErrorMessage = " Fullname to be at least 5 characters!!")]
        [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
        public string Fullname { get; set; } = null!;

        public string? Image { get; set; }
       // public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
        [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Address cannot be empty!!")]
        public string Address { get; set; } = null!;

        public bool? IsBan { get; set; }

    }

    public class OwnerAvatarDTO
    {
        public int OwnerId { get; set; }
        public string? Image { get; set; }
        //[NotMapped]
      //  public IFormFile? ImageFile { get; set; }
    }

    public class OwnerProfileDTO
    {
        public int OwnerId { get; set; }
        [Required(ErrorMessage = " Email cannot be empty!!")]
        [EmailAddress]
        [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = " Fullname cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
        [MaxLength(30, ErrorMessage = "Fullname is limited to 30 characters!!")]
        public string Fullname { get; set; } = null!;

        //  public string? Image { get; set; }
        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
        [MaxLength(13, ErrorMessage = "Phone is limited to 13 characters!!")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = " Address cannot be empty!!")]
        public string Address { get; set; } = null!;

    }
}
