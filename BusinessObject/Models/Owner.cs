using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public partial class Owner
{
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OwnerId { get; set; }

    [Required(ErrorMessage = " Email cannot be empty!!")]
    [EmailAddress]
    [MinLength(2, ErrorMessage = " Email to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Email is limited to 50 characters!!")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = " Password cannot be empty!!")]
    [MinLength(6, ErrorMessage = " Password to be at least 6 characters!!")]
    [MaxLength(10, ErrorMessage = "Password is limited to 10 characters!!")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = " Fullname cannot be empty!!")]
    [MinLength(5, ErrorMessage = " Fullname to be at least 5 characters!!")]
    [MaxLength(50, ErrorMessage = "Fullname is limited to 50 characters!!")]
    public string Fullname { get; set; } = null!;

    public string? Image { get; set; }

    [Required(ErrorMessage = " Phone cannot be empty!!")]
    [MinLength(9, ErrorMessage = " Phone to be at least 9 characters!!")]
    [MaxLength(10, ErrorMessage = "Phone is limited to 10 characters!!")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = " Address cannot be empty!!")]
    public string Address { get; set; } = null!;

    public bool IsBan { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GuestConsultation> GuestConsultations { get; set; } = new List<GuestConsultation>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
