using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? AccountId { get; set; }

    public int? OwnerId { get; set; }

    public string ContentNotification { get; set; } = null!;

    public bool IsRead { get; set; }

    public string? Url { get; set; }

    public DateTime NotificationCreatedDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Owner? Owner { get; set; }
}
