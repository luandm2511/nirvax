using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Image { get; set; }

    public string Phone { get; set; } = null!;

    public int OwnerId { get; set; }

    public virtual Owner Owner { get; set; } = null!;
}
