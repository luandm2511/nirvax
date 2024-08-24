using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public string ContentMessage { get; set; } = null!;

    public DateTime MessageTimestamp { get; set; }

    public int RoomId { get; set; }

    public string Sendertype { get; set; } = null!;
}
