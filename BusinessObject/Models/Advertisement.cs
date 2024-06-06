using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Advertisement
{
    [Key]
    [Required(ErrorMessage = " AdId cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AdId { get; set; }

    [Required(ErrorMessage = " Title cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
    [MaxLength(100, ErrorMessage = " Title to be at least 100 characters!!")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = " Fullname cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Fullname to be at least 2 characters!!")]
    [MaxLength(4000, ErrorMessage = "Fullname is limited to 4000 characters!!")]
    public string Content { get; set; } = null!;

    public string Image { get; set; } = null!;
    [Required(ErrorMessage = " StatusPostId cannot be empty!!")]
    public int StatusPostId { get; set; }
    [Required(ErrorMessage = " ServiceId cannot be empty!!")]
    public int ServiceId { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]
    public int OwnerId { get; set; }

    public virtual ICollection<GuestConsultation> GuestConsultations { get; set; } = new List<GuestConsultation>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual PostStatus StatusPost { get; set; } = null!;
}
