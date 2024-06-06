using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Room
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoomId { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]

    public int AccountId { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]

    public int OwnerId { get; set; }
    [Required(ErrorMessage = " Content cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
    [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Owner Owner { get; set; } = null!;
}
