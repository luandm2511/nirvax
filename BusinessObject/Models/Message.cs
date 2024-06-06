using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Message
{
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }
    [Required(ErrorMessage = " SenderId cannot be empty!!")]
    public int SenderId { get; set; }
    [Required(ErrorMessage = " ReceiverId cannot be empty!!")]

    public int? ReceiverId { get; set; }
    [Required(ErrorMessage = " Content cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Content to be at least 2 characters!!")]
    [MaxLength(500, ErrorMessage = "Content is limited to 500 characters!!")]
    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }
    [Required(ErrorMessage = " RoomId cannot be empty!!")]

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;
}
