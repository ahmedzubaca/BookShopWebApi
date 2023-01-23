using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.DTO.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public double? ListPrice { get; set; }
        public double? Price { get; set; }
        public double? Price50 { get; set; }
        public double? Price100 { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int? CoverTypeId { get; set; }
    }
}
