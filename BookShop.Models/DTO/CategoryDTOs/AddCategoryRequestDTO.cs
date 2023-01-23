using System.ComponentModel.DataAnnotations;

namespace BookShop.Models.DTO.CategoryDTOs
{
    public class AddCategoryRequestDTO
    {
        [Required]
        public AddCategoryDTO? Category { get; set; }
    }
}
