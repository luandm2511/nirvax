using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class GuestConsultation
{
    public int GuestId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int StatusGuestId { get; set; }

    public int AdId { get; set; }

    public int OwnerId { get; set; }

    public virtual Advertisement Ad { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual GuestStatus StatusGuest { get; set; } = null!;
}
