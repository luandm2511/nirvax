﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int AccountId { get; set; }

    public int OwnerId { get; set; }

    public string ContentLastMessage { get; set; } = null!;

    public DateTime LastMessageTimestamp { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;
}
