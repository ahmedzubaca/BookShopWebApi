using AutoMapper;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.Domain;
using BookShop.Models.DTO.CompanyDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShopWeb.Controllers
{
    [Route("companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CompaniesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var companiesDb = await unitOfWork.Company.GetAllAsync();

            var companiesDTO = companiesDb.Select( company => 
             mapper.Map<CompanyDTO>(company)
            ).ToList();

            var getAllCompanies = new GetAllCompaniesResponse { Companies= companiesDTO };

            return Ok(getAllCompanies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCompanyByIdAsync(int id)
        {
            var companyDb = await unitOfWork.Company.GetFirstOrDefaultAsync(c => c.Id == id);
            if (companyDb == null)
            {
                return NotFound();
            }

            var companyDTO = mapper.Map<CompanyDTO>(companyDb);
            var getSingleCompanyResponse = new GetSingleCompanyResponse { Company = companyDTO };

            return Ok(getSingleCompanyResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyAsync([FromBody]AddCompanyRequestDTO addCompanyRequest)
        {
            if(addCompanyRequest.Company == null)
            {
                return BadRequest();
            }

            var addCompany = addCompanyRequest.Company;

            var companiDomain = new Company
            {
                Name = addCompany.Name,
                StreetAddress = addCompany.StreetAddress,
                City = addCompany.City,
                PostalCode = addCompany.PostalCode,
                State = addCompany.State,
                PhoneNumber = addCompany.PhoneNumber
            };
            unitOfWork.Company.Add(companiDomain);
            await unitOfWork.SaveAsync();

            var companyDTO = mapper.Map<CompanyDTO>(companiDomain);
            var singleCompanyResponse = new GetSingleCompanyResponse { Company = companyDTO };

            return CreatedAtAction("GetCompanyById", new {id = singleCompanyResponse.Company.Id }, singleCompanyResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody]UpdateCompanyRequest updateCompanyRequest)
        {
            if(updateCompanyRequest.Company == null)
            {
                return BadRequest();
            }
            var companyDB = await unitOfWork.Company.GetFirstOrDefaultAsync(c => c.Id == id);
            if(companyDB == null)
            {
                return NotFound();
            }

            var reqCompany = updateCompanyRequest.Company;
            
            if(reqCompany.Name != null) companyDB.Name = reqCompany.Name;
            if(reqCompany.StreetAddress != null) companyDB.StreetAddress = reqCompany.StreetAddress;
            if (reqCompany.City != null) companyDB.StreetAddress = reqCompany.StreetAddress;
            if(reqCompany.State != null) companyDB.State = reqCompany.State;
            if(reqCompany.PostalCode != null) companyDB.PostalCode = reqCompany.PostalCode;
            if(reqCompany.PhoneNumber != null) companyDB.PhoneNumber = reqCompany.PhoneNumber;

            unitOfWork.Company.Update(companyDB);
            await unitOfWork.SaveAsync();

            var companyDTO = mapper.Map<CompanyDTO>(companyDB);
            var singleCompanyResponse = new GetSingleCompanyResponse { Company = companyDTO };

            return CreatedAtAction("GetCompanyById", new { id = singleCompanyResponse.Company.Id }, singleCompanyResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var companyDb = await unitOfWork.Company.GetFirstOrDefaultAsync(c => c.Id == id);
            if(companyDb == null) return NotFound();

            unitOfWork.Company.Remove(companyDb);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("deleterange")]

        public async Task<IActionResult> DeleteRange(List<Company> companiesList)
        {
            unitOfWork.Company.RemoveRange(companiesList);
            await unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
