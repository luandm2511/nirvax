using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Login, Account>().ReverseMap();
            CreateMap<AccountGoogle, Account>().ReverseMap();
            CreateMap<Login, Owner>().ReverseMap();
            CreateMap<Login, Staff>().ReverseMap();
            CreateMap<Account, UpdateUserDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryParent, CateParentDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Comment, ReplyCommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>();
            CreateMap<Order, OrderItemDTO>();

            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<Owner, OwnerAvatarDTO>().ReverseMap();
            CreateMap<Owner, OwnerProfileDTO>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<SizeChart, SizeChartDTO>().ReverseMap();
            CreateMap<SizeChart, SizeChartCreateDTO>().ReverseMap();



            CreateMap<Staff, StaffAvatarDTO>().ReverseMap();
            CreateMap<Staff, StaffCreateDTO>().ReverseMap();

            CreateMap<Staff, StaffProfileDTO>().ReverseMap();
            CreateMap<Size, SizeDTO>()
             // .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Fullname))
                .ReverseMap();
            CreateMap<Size, SizeCreateDTO>().ReverseMap();

            CreateMap<Advertisement, AdvertisementDTO>()
              //  .ForMember(dest => dest.StatusPostName, opt => opt.MapFrom(src => src.StatusPost.Name))
              //  .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
              //  .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Fullname))
                .ReverseMap();
            CreateMap<Advertisement, AdvertisementCreateDTO>().ReverseMap();
            

            

            CreateMap<GuestConsultation, GuestConsultationDTO>()           
                .ReverseMap();

            CreateMap<GuestConsultation, GuestConsultationCreateDTO>().ReverseMap();


            CreateMap<ImportProduct, ImportProductDTO>().ReverseMap();
            CreateMap<ImportProduct, ImportProductCreateDTO>().ReverseMap();

            CreateMap<ImportProductDetail, ImportProductDetailDTO>().ReverseMap();
            CreateMap<ImportProductDetail, ImportProductDetailCreateDTO>().ReverseMap();
            CreateMap<ImportProductDetail, ImportProductDetailUpdateDTO>().ReverseMap();
            CreateMap<ImportProductDetail, ImportProductDetailByImportDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductSize.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductSize.Product.Name))
                .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.ProductSize.Size.SizeId))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductSize.Size.Name))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Import.Origin))
                .ForMember(dest => dest.ImportDate, opt => opt.MapFrom(src => src.Import.ImportDate))
                .ReverseMap();

            





            CreateMap<ProductSize, ProductSizeDTO>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name))
                .ReverseMap();

            CreateMap<ProductSize, ProductSizeCreateDTO>().ReverseMap();
            CreateMap<ProductSize, ProductSizeListDTO>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
             
                .AfterMap((src, dest) =>
                {
                    dest.Status = src.Quantity == 0 ? "sold out" : "instock";
                })
                .ReverseMap();





            CreateMap<BusinessObject.Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<BusinessObject.Models.Service, ServiceCreateDTO>().ReverseMap();

            CreateMap<Voucher, VoucherDTO>()            
                .ReverseMap();
            CreateMap<Voucher, VoucherCreateDTO>().ReverseMap();


            CreateMap<Room, RoomDTO>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Fullname))             
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Fullname))
            .ForMember(dest => dest.OwnerImage, opt => opt.MapFrom(src => src.Owner.Image))
            .ForMember(dest => dest.AccountImage, opt => opt.MapFrom(src => src.Account.Image))

                .ReverseMap();
            CreateMap<Room, RoomCreateDTO>().ReverseMap();
            CreateMap<Room, RoomContentDTO>().ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Message, MessageCreateDTO>().ReverseMap();

        }
    }
}
