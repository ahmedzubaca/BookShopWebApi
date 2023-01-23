using BookShop.DataAccess.Repository;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.Domain;
using BookShop.Models.DTO.CoverTypeDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookShopWeb.Controllers
{
    [Route("covertypes")]
    [ApiController]
    public class CoverTypesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CoverTypesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoverTypesAsync()
        {
            var coverTypes = await unitOfWork.CoverType.GetAllAsync();
            if (coverTypes == null)
            {
                return NotFound();
            }
            var coverTypesDTO = coverTypes.Select(coverType => new CoverTypeDTO
            {
                Id = coverType.Id,
                Name = coverType.Name,
            }).ToList();

            return Ok(new GetAllCoverTypesResponse { CoverTypes = coverTypesDTO });
        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("GetCoverTypeById")]
        public async Task<IActionResult> GetCoverTypeByIdAsync(int id)
        {
            var coverTypeDb = await unitOfWork.CoverType.GetFirstOrDefaultAsync(x => x.Id == id);
            if (coverTypeDb == null)
            {
                return NotFound();
            }
            var coverTypeDTO = new CoverTypeDTO
            {
                Id = coverTypeDb.Id,
                Name = coverTypeDb.Name
            };

            return Ok(new GetSingleCoverTypeResponse { CoverType = coverTypeDTO });
        }

        [HttpPost]
        public async Task<IActionResult> AddCoverTypeAsync(AddRequestCoverTypeDTO reqCoverType)
        {
            var coverTypeDomain = new CoverType
            {
                Name = reqCoverType.Name,
            };
            unitOfWork.CoverType.Add(coverTypeDomain);
            await unitOfWork.SaveAsync();

            var coverTypeDTO = new CoverTypeDTO
            {
                Id = coverTypeDomain.Id,
                Name = coverTypeDomain.Name
            };

            var singleCoverTypeResponse = new GetSingleCoverTypeResponse { CoverType = coverTypeDTO };

            return CreatedAtAction("GetCoverTypeById", new { id = singleCoverTypeResponse.CoverType.Id }, singleCoverTypeResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoverType(int id, [FromBody] AddRequestCoverTypeDTO request)
        {
            var coverTypeToUpdate = await unitOfWork.CoverType.GetFirstOrDefaultAsync(ct => ct.Id == id);
            if (coverTypeToUpdate == null)
            {
                return BadRequest();
            }

            coverTypeToUpdate.Name = request.Name;
            await unitOfWork.SaveAsync();

            var coverTypeDTO = new CoverTypeDTO
            {
                Id = coverTypeToUpdate.Id,
                Name = coverTypeToUpdate.Name
            };

            var singleCoverTypeResponse = new GetSingleCoverTypeResponse { CoverType = coverTypeDTO };

            return CreatedAtAction("GetCoverTypeById", new { id = singleCoverTypeResponse.CoverType.Id }, singleCoverTypeResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCoverType(int id)
        {
            var coverTypeToDelete = await unitOfWork.CoverType.GetFirstOrDefaultAsync(ct => ct.Id == id);
            if (coverTypeToDelete == null)
            {
                return NotFound();
            }
            unitOfWork.CoverType.Remove(coverTypeToDelete);
            await unitOfWork.SaveAsync();

            var coverTypeDTO = new CoverTypeDTO
            {
                Id = coverTypeToDelete.Id,
                Name = coverTypeToDelete.Name
            };
            var singleCoverTypeResponse = new GetSingleCoverTypeResponse { CoverType = coverTypeDTO };
            return Ok(singleCoverTypeResponse);
        }

        [HttpDelete]
        [Route("deleterange")]

        public async Task<IActionResult> DeleteRange(List<CoverType> coverTypesList)
        {
            unitOfWork.CoverType.RemoveRange(coverTypesList);
            await unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
