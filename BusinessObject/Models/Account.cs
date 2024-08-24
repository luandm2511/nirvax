using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? UserImage { get; set; }

    public string UserPhone { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Gender { get; set; } = null!;

    public string UserAddress { get; set; } = null!;

    public DateTime? UserCreatedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string Role { get; set; } = null!;

    public bool IsBan { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
