using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string StaffEmail { get; set; } = null!;

    public string StaffPassword { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string? StaffImage { get; set; }

    public string StaffPhone { get; set; } = null!;

    public int OwnerId { get; set; }

    public virtual Owner Owner { get; set; } = null!;
}
