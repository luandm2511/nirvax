using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string OwnerEmail { get; set; } = null!;

    public string OwnerPassword { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public string? OwnerImage { get; set; }

    public string OwnerPhone { get; set; } = null!;

    public string OwnerAddress { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public bool IsBan { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GuestConsultation> GuestConsultations { get; set; } = new List<GuestConsultation>();

    public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new List<ImportProduct>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<SizeChart> SizeCharts { get; set; } = new List<SizeChart>();

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
