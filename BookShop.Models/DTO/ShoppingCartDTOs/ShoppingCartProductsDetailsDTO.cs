using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ShoppingCartDTOs
{
    public class ShoppingCartProductsDetailsDTO
    {
        public string? Title { get; set; }        
        public double Price { get; set; }        
        public int Count { get; set; }
        public double? ChosenProductsPrice { get; set; }
        public string? ImageUrl { get; set; }
    }
}
