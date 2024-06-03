using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Models;

public partial class NirvaxContext : DbContext
{
    public NirvaxContext()
    {
    }

    public NirvaxContext(DbContextOptions<NirvaxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLog> AccessLogs { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Description> Descriptions { get; set; }

    public virtual DbSet<GuestConsultation> GuestConsultations { get; set; }

    public virtual DbSet<GuestStatus> GuestStatuses { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImportProduct> ImportProducts { get; set; }

    public virtual DbSet<ImportProductDetail> ImportProductDetails { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<PostStatus> PostStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseDetail> WarehouseDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HKH7PJO\\MSSQLSERVER01;Initial Catalog=Nirvax;User ID=sa;Password=123;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;TrustServerCertificate=true;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLog>(entity =>
        {
            entity.HasKey(e => e.AccessId);

            entity.Property(e => e.AccessId).HasColumnName("access_id");
            entity.Property(e => e.AccessTime)
                .HasColumnType("datetime")
                .HasColumnName("access_time");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ip_address");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_agent");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CD0D8015C3");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.IsBan).HasColumnName("is_ban");
            entity.Property(e => e.Password)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("PK__Advertis__CAA4A62737D7E1DD");

            entity.ToTable("Advertisement");

            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.StatusPostId).HasColumnName("status_post_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Owner).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_advertisement_owner");

            entity.HasOne(d => d.Service).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_advertisement_service");

            entity.HasOne(d => d.StatusPost).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.StatusPostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_advertisement_poststatus");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__5E5A8E274E00B600");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Brands)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_brand_category");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B46EDD1701");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__E795768716814496");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Reply)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("reply");
            entity.Property(e => e.ReplyTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("reply_timestamp");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comment_account");

            entity.HasOne(d => d.Owner).WithMany(p => p.Comments)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("fk_comment_owner");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comment_product");
        });

        modelBuilder.Entity<Description>(entity =>
        {
            entity.ToTable("Description");

            entity.Property(e => e.DescriptionId).HasColumnName("description_id");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<GuestConsultation>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PK__GuestCon__19778E3531EC48AB");

            entity.ToTable("GuestConsultation");

            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.StatusGuestId).HasColumnName("status_guest_id");

            entity.HasOne(d => d.Ad).WithMany(p => p.GuestConsultations)
                .HasForeignKey(d => d.AdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_guestconsultation_advertisement");

            entity.HasOne(d => d.Owner).WithMany(p => p.GuestConsultations)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_guestconsultation_owner");

            entity.HasOne(d => d.StatusGuest).WithMany(p => p.GuestConsultations)
                .HasForeignKey(d => d.StatusGuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_guestconsultation_gueststatus");
        });

        modelBuilder.Entity<GuestStatus>(entity =>
        {
            entity.HasKey(e => e.StatusGuestId).HasName("PK__GuestSta__1ACF0575AC6339CC");

            entity.ToTable("GuestStatus");

            entity.Property(e => e.StatusGuestId)
                .ValueGeneratedNever()
                .HasColumnName("status_guest_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK_Image_1");

            entity.ToTable("Image");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.DescriptionId).HasColumnName("description_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.LinkImage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("link_image");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Description).WithMany(p => p.Images)
                .HasForeignKey(d => d.DescriptionId)
                .HasConstraintName("fk_image_description");

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_image_product");
        });

        modelBuilder.Entity<ImportProduct>(entity =>
        {
            entity.HasKey(e => e.ImportId).HasName("PK__ImportPr__F3E6B05F2EFB601C");

            entity.ToTable("ImportProduct");

            entity.Property(e => e.ImportId).HasColumnName("import_id");
            entity.Property(e => e.ImportDate)
                .HasColumnType("datetime")
                .HasColumnName("import_date");
            entity.Property(e => e.Origin)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("origin");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ImportProducts)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_importproduct_warehouse");
        });

        modelBuilder.Entity<ImportProductDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ImportProductDetail");

            entity.Property(e => e.ImportId).HasColumnName("import_id");
            entity.Property(e => e.ProductSizeId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("product_size_id");
            entity.Property(e => e.QuantityReceived).HasColumnName("quantity_received");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Import).WithMany()
                .HasForeignKey(d => d.ImportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_importproductdetail_importproduct");

            entity.HasOne(d => d.ProductSize).WithMany()
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_importproductdetail_productsize");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__0BBF6EE65EE8AE97");

            entity.ToTable("Message");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Room).WithMany(p => p.Messages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_message_room");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FE7BF43ED");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_notification_account");

            entity.HasOne(d => d.Owner).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("fk_notification_owner");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__46596229DEBCF00D");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CodeOrder)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code_order");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.RequiredDate)
                .HasColumnType("datetime")
                .HasColumnName("required_date");
            entity.Property(e => e.ShippedDate)
                .HasColumnType("datetime")
                .HasColumnName("shipped_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.VoucherId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("voucher_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_account");

            entity.HasOne(d => d.Owner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_owner");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_status");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("fk_order_voucher");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductSizeId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("product_size_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_order");

            entity.HasOne(d => d.ProductSize).WithMany()
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_productsize");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__3683B531F1692557");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owner__AD081786280DDB20");

            entity.ToTable("Owner");

            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.IsBan).HasColumnName("is_ban");
            entity.Property(e => e.Password)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<PostStatus>(entity =>
        {
            entity.HasKey(e => e.StatusPostId).HasName("PK__PostStat__0271DBBF8F3DF3F0");

            entity.ToTable("PostStatus");

            entity.Property(e => e.StatusPostId).HasColumnName("status_post_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5AC8E29A4");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DescriptionId).HasColumnName("description_id");
            entity.Property(e => e.Isban).HasColumnName("isban");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");
            entity.Property(e => e.RateCount).HasColumnName("rate_count");
            entity.Property(e => e.RatePoint).HasColumnName("rate_point");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("short_description");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_product_brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_category");

            entity.HasOne(d => d.Description).WithMany(p => p.Products)
                .HasForeignKey(d => d.DescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_description");

            entity.HasOne(d => d.Owner).WithMany(p => p.Products)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_owner");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.ProductSizeId).HasName("PK__ProductS__062A9A68C2CE90EA");

            entity.ToTable("ProductSize");

            entity.Property(e => e.ProductSizeId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("product_size_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("size_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productsize_product");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productsize_size");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19675A8A174AB013");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Account).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_room_account");

            entity.HasOne(d => d.Owner).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_room_owner");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AFEABC0315");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Size__0DCACE3173588E43");

            entity.ToTable("Size");

            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Sizes)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_size_ower");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__1963DD9C124845EC");

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Password)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.HasOne(d => d.Owner).WithMany(p => p.Staff)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_staff_owner");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Voucher__80B6FFA8083E8B23");

            entity.ToTable("Voucher");

            entity.Property(e => e.VoucherId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("voucher_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_voucher_owner");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__734FE6BF81DFF54B");

            entity.ToTable("Warehouse");

            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");

            entity.HasOne(d => d.Owner).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_warehouse_owner");
        });

        modelBuilder.Entity<WarehouseDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("WarehouseDetail");

            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.ProductSizeId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("product_size_id");
            entity.Property(e => e.QuantityInStock).HasColumnName("quantity_in_stock");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.ProductSize).WithMany()
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_warehousedetail_productsize");

            entity.HasOne(d => d.Warehouse).WithMany()
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_warehousedetail_warehouse");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
