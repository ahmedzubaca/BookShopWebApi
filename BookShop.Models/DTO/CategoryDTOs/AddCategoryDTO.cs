using System.ComponentModel.DataAnnotations;

namespace BookShop.Models.DTO.CategoryDTOs
{
    public class AddCategoryDTO
    {
        [Required]
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
