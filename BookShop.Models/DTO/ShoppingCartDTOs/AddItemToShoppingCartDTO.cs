using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ShoppingCartDTOs
{
    public class AddItemToShoppingCartDTO
    {
        [Required]
        public string? UserId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
       
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}
