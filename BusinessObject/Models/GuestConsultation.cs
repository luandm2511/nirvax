using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class GuestConsultation
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GuestId { get; set; }
    [Required(ErrorMessage = " Fullname cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
    public string Fullname { get; set; } = null!;
    [Required(ErrorMessage = " Phone cannot be empty!!")]
    [MinLength(8, ErrorMessage = " Phone to be at least 8 characters!!")]
    [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
    public string Phone { get; set; } = null!;
    [Required(ErrorMessage = " Content cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
    [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
    public string Content { get; set; } = null!;
    [Required(ErrorMessage = " StatusGuestId cannot be empty!!")]

    public int StatusGuestId { get; set; }
    [Required(ErrorMessage = " AdId cannot be empty!!")]

    public int AdId { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]

    public int OwnerId { get; set; }

    public virtual Advertisement Ad { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual GuestStatus StatusGuest { get; set; } = null!;
}
