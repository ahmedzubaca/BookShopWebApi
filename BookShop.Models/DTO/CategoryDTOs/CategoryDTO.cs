using System.ComponentModel.DataAnnotations;

namespace BookShop.Models.DTO.CategoryDTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}
