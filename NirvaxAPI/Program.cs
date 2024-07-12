using System.Runtime.ConstrainedExecution;
using System.Text;
using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using WebAPI.Helpers;
using WebAPI.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddDbContext<NirvaxContext>(options =>
                                                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<AuthenticationDAO>();
builder.Services.AddScoped<BrandDAO>();
builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<CategoryParentDAO>();
builder.Services.AddScoped<CommentDAO>();
builder.Services.AddScoped<ImageDAO>();
builder.Services.AddScoped<NotificationDAO>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<OrderDetailDAO>();
builder.Services.AddScoped<OwnerDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<ProductSizeDAO>();
builder.Services.AddScoped<VoucherDAO>();
builder.Services.AddScoped<StaffDAO>();
builder.Services.AddScoped<AccessLogDAO>();

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICateParentRepository, CateParentRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IAccessLogRepository, AccessLogRepository>();
builder.Services.AddScoped<IAccessLogService, AccessLogService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<StaffDAO>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<OwnerDAO>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<SizeDAO>();

builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
builder.Services.AddScoped<ProductSizeDAO>();



builder.Services.AddScoped<IImportProductDetailRepository, ImportProductDetailRepository>();
builder.Services.AddScoped<ImportProductDetailDAO>();

builder.Services.AddScoped<IImportProductRepository, ImportProductRepository>();
builder.Services.AddScoped<ImportProductDAO>();

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<WarehouseDAO>();


builder.Services.AddScoped<IWarehouseDetailRepository, WarehouseDetailRepository>();
builder.Services.AddScoped<WarehouseDetailDAO>();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ServiceDAO>();

builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<VoucherDAO>();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<RoomDAO>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageDAO>();


builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<AdvertisementDAO>();



builder.Services.AddScoped<IDescriptionRepository, DescriptionRepository>();
builder.Services.AddScoped<DescriptionDAO>();
builder.Services.AddScoped<NotificationDAO>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IGuestConsultationRepository, GuestConsultationRepository>();
builder.Services.AddScoped<GuestConsultationDAO>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidateLifetime = true, // Xác nhận thời gian hết hạn của token
        ClockSkew = TimeSpan.Zero, // Không chấp nhận độ chệch thời gian
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
}).AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/api/GoogleLogin/GoogleResponse";
    options.SaveTokens = true;
});
builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAll");

app.UseSession();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAccessLogMiddleware();

app.MapControllers();

app.Run();
