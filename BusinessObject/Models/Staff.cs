using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public partial class Staff
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StaffId { get; set; }

    [Required(ErrorMessage = " Email cannot be empty!!")]
    [EmailAddress]
    [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = " Password cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Password to be at least 6 characters!!")]
    [MaxLength(50, ErrorMessage = "Password is limited to 10 characters!!")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = " Fullname cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Fullname is limited to 30 characters!!")]
    public string Fullname { get; set; } = null!;

   
    public string? Image { get; set; }

    [Required(ErrorMessage = " Phone cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Phone to be at least 9 characters!!")]
    [MaxLength(50, ErrorMessage = "Phone is limited to 13 characters!!")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = " Owner cannot be empty!!")]
    public int OwnerId { get; set; }

    public virtual Owner Owner { get; set; } = null!;
}
