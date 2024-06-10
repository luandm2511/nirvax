using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class AccessLog
{
    public int AccessId { get; set; }

    public DateTime AccessTime { get; set; }

    public string IpAddress { get; set; } = null!;

    public string UserAgent { get; set; } = null!;
}
