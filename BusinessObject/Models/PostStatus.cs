using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class PostStatus
{
    public int StatusPostId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
