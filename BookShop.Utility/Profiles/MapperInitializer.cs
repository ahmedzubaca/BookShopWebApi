using AutoMapper;
using BookShop.Models.Domain;
using BookShop.Models.DTO.ProductDTOs;
using BookShop.Models.DTO.ShoppingCartDTOs;

namespace BookShopWeb.Profiles
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, OfferedProductDetailsDTO>().ReverseMap();
        }
    }
}
