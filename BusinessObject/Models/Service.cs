using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Service
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ServiceId { get; set; }
    [Required(ErrorMessage = " Name cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
    public string Name { get; set; } = null!;

    public bool Isdelete { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
