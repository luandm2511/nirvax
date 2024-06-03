using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;
}
