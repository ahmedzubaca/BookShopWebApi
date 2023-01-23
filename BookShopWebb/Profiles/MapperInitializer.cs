using AutoMapper;
using BookShop.DataAccess.Data;
using BookShop.Models.Domain;
using BookShop.Models.DTO.CompanyDTOs;
using BookShop.Models.DTO.ProductDTOs;
using BookShop.Models.DTO.UserDTOs;
namespace BookShopWeb.Profiles
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
        }
    }
}
