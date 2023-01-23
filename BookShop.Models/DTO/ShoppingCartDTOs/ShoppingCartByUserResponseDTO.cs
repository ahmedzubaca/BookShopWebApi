using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ShoppingCartDTOs
{
    public class ShoppingCartByUserResponseDTO
    {
        [Display(Name = "Shopping Cart")]
        public List<ShoppingCartProductsDetailsDTO>? ListShoppingCart { get; set; }
        public double TotalPrice { get; set; }
    }
}
