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
            CreateMap<Login, Owner>().ReverseMap();
            CreateMap<Login, Staff>().ReverseMap();
            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<Owner, OwnerAvatarDTO>().ReverseMap();

            CreateMap<Owner, OwnerProfileDTO>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Description, DescriptionDTO>().ReverseMap();

            // CreateMap<Staff, StaffCreateDTO>().ReverseMap();
            //   CreateMap<Staff, StaffBaseDTO>().ReverseMap();

            CreateMap<Staff, StaffAvatarDTO>().ReverseMap();

            CreateMap<Staff, StaffProfileDTO>().ReverseMap();
            CreateMap<Size, SizeDTO>().ReverseMap();
            CreateMap<Advertisement, AdvertisementDTO>().ReverseMap();
            CreateMap<Advertisement, AdvertisementCreateDTO>().ReverseMap();


            CreateMap<GuestConsultation, GuestConsultationDTO>().ReverseMap();

            CreateMap<ImportProduct, ImportProductDTO>().ReverseMap();
            CreateMap<ImportProductDetail, ImportProductDetailDTO>().ReverseMap();
            CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
           CreateMap<WarehouseDetail, WarehouseDetailFinalDTO>().ReverseMap();
            CreateMap<WarehouseDetail, WarehouseDetailDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductSize, ProductSizeDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Service, ServiceDTO>().ReverseMap();
            CreateMap<Voucher, VoucherDTO>().ReverseMap();
        

            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Room, RoomContentDTO>().ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();







        }
    }
}
