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
            CreateMap<Description, DescriptionDTO>().ReverseMap();
            CreateMap<Description, DescriptionCreateDTO>().ReverseMap();




            CreateMap<Staff, StaffAvatarDTO>().ReverseMap();
            CreateMap<Staff, StaffCreateDTO>().ReverseMap();

            CreateMap<Staff, StaffProfileDTO>().ReverseMap();
            CreateMap<Size, SizeDTO>().ReverseMap();
            CreateMap<Size, SizeCreateDTO>().ReverseMap();

            CreateMap<Advertisement, AdvertisementDTO>().ReverseMap();
            CreateMap<Advertisement, AdvertisementCreateDTO>().ReverseMap();


            CreateMap<GuestConsultation, GuestConsultationDTO>().ReverseMap();
            CreateMap<GuestConsultation, GuestConsultationCreateDTO>().ReverseMap();


            CreateMap<ImportProduct, ImportProductDTO>().ReverseMap();
            CreateMap<ImportProduct, ImportProductCreateDTO>().ReverseMap();

            CreateMap<ImportProductDetail, ImportProductDetailDTO>().ReverseMap();
            

            CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
            CreateMap<Warehouse, WarehouseCreateDTO>().ReverseMap();

            CreateMap<WarehouseDetail, WarehouseDetailFinalDTO>().ReverseMap();
            CreateMap<WarehouseDetail, WarehouseDetailDTO>().ReverseMap();

       
            CreateMap<ProductSize, ProductSizeDTO>().ReverseMap();
            CreateMap<ProductSize, ProductSizeCreateDTO>().ReverseMap();



            CreateMap<BusinessObject.Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<BusinessObject.Models.Service, ServiceCreateDTO>().ReverseMap();

            CreateMap<Voucher, VoucherDTO>().ReverseMap();
            CreateMap<Voucher, VoucherCreateDTO>().ReverseMap();


            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Room, RoomCreateDTO>().ReverseMap();
            CreateMap<Room, RoomContentDTO>().ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Message, MessageCreateDTO>().ReverseMap();

        }
    }
}
