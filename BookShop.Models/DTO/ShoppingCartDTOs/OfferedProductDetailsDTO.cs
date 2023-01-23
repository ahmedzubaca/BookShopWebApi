using BookShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ShoppingCartDTOs
{
    public class OfferedProductDetailsDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public double ListPrice { get; set; }
        public double Price { get; set; }
        public double Price50 { get; set; }
        public double Price100 { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public string? CoverType { get; set; }
        public int Count { get; set; }
    }
}
