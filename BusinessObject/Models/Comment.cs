using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int AccountId { get; set; }

    public int ProductId { get; set; }

    public int? OwnerId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string? Reply { get; set; }

    public DateTime? ReplyTimestamp { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Owner? Owner { get; set; }

    public virtual Product Product { get; set; } = null!;
}
