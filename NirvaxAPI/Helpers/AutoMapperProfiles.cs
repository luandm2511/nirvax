using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Login, Account>().ReverseMap();
            CreateMap<Login, Owner>().ReverseMap();
            CreateMap<Login, Staff>().ReverseMap();
            CreateMap<Account, UpdateUserDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore()); // Image sẽ được xử lý riêng
            CreateMap<Brand, BrandDTO>().ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore()); // Image sẽ được xử lý riêng
            CreateMap<Product, ProductDTO>().ReverseMap()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Comment, ReplyCommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
