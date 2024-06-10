using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class GuestStatus
{
    public int StatusGuestId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GuestConsultation> GuestConsultations { get; set; } = new List<GuestConsultation>();
}
