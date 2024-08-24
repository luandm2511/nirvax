using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Advertisement
{
    public int AdId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int StatusPostId { get; set; }

    public int ServiceId { get; set; }

    public int OwnerId { get; set; }

    public virtual ICollection<GuestConsultation> GuestConsultations { get; set; } = new List<GuestConsultation>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual PostStatus StatusPost { get; set; } = null!;
}
