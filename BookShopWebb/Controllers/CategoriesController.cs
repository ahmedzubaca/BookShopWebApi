

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShop.DataAccess.Data;
using BookShop.Models.Domain;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.DTO.CategoryDTOs;

namespace BookShopWebb.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {        
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {            
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
         public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.Category.GetAllAsync();

            if (!categories.Any())
            {
                return NotFound();
            }

            var categoriesDTOList = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
                CreatedDateTime = category.CreatedDateTime,
                UpdatedDateTime = category.UpdatedDateTime,
            }).ToList();

            return Ok(new AllCategoriesResponseDTO { Categories = categoriesDTOList});
        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("GetCategoryById")]

        public async Task<IActionResult> GetCategoryByIdAsync([FromRoute]int id)
        {
            var category = await unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
                CreatedDateTime = category.CreatedDateTime,
                UpdatedDateTime = category.UpdatedDateTime,
            };

            return Ok(new SingleCategoryResponseDTO { Category = categoryDTO });
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync([FromBody] AddCategoryRequestDTO request)
        {
            if(!ModelState.IsValid || request.Category!.DisplayOrder == 0)
            {
                return BadRequest("None of the fields can be empty or '0'");
            }
            var reqCategory = request.Category;            

            var categoryDomain = new Category
            {
                Name = reqCategory!.Name,
                DisplayOrder = reqCategory.DisplayOrder,
                CreatedDateTime = DateTime.UtcNow
            };
            unitOfWork.Category.Add(categoryDomain);
            await unitOfWork.SaveAsync();

            var categoryDTO = new CategoryDTO
            {
                Id = categoryDomain.Id,
                Name = categoryDomain.Name,
                DisplayOrder = categoryDomain.DisplayOrder,
                CreatedDateTime = categoryDomain.CreatedDateTime,
                UpdatedDateTime = categoryDomain.UpdatedDateTime
            };

            var singleCategoryResultDTO = new SingleCategoryResponseDTO { Category = categoryDTO };
            
            return CreatedAtAction("GetCategoryById", new { id = singleCategoryResultDTO.Category.Id }, singleCategoryResultDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var categoryToUpdate = await unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            var reqCategory = request.Category;

            var updatedCategoryName = reqCategory!.Name ?? string.Empty;
            if (updatedCategoryName.Length > 0)
            {
                categoryToUpdate.Name = updatedCategoryName;
            }

            var updatedDisplayOrder = reqCategory.DisplayOrder ?? 0;
            if (updatedDisplayOrder > 0)
            {
                categoryToUpdate.DisplayOrder = updatedDisplayOrder;
            }

            if (updatedCategoryName.Length > 0 || updatedDisplayOrder != 0)
            {
                categoryToUpdate.UpdatedDateTime = DateTime.Now;
                await unitOfWork.SaveAsync();

                var categoryDTO = new CategoryDTO
                {
                    Id = categoryToUpdate.Id,
                    Name = categoryToUpdate.Name,
                    DisplayOrder = categoryToUpdate.DisplayOrder,
                    CreatedDateTime = categoryToUpdate.CreatedDateTime,
                    UpdatedDateTime = DateTime.UtcNow
                };

                var singleCategoryResponse = new SingleCategoryResponseDTO { Category = categoryDTO };

                return CreatedAtAction("GetCategoryById", new { id = singleCategoryResponse.Category.Id }, singleCategoryResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToDelete = await unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }
            unitOfWork.Category.Remove(categoryToDelete);
            await unitOfWork.SaveAsync();
            var categoryDTO = new CategoryDTO
            {
                Id = categoryToDelete.Id,
                Name = categoryToDelete.Name,
                DisplayOrder = categoryToDelete.DisplayOrder,
                CreatedDateTime = categoryToDelete.CreatedDateTime,
                UpdatedDateTime = categoryToDelete.CreatedDateTime
            };

            var singleCategoryResponse = new SingleCategoryResponseDTO { Category = categoryDTO };

            return Ok(singleCategoryResponse);
        }

        [HttpDelete]
        [Route("deleterange")]

        public async Task<IActionResult> DeleteRange(List<Category> categoriesList)
        {
            unitOfWork.Category.RemoveRange(categoriesList);
            await unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
