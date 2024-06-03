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
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Comment, ReplyCommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
