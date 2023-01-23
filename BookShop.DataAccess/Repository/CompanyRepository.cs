using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext db;
        public CompanyRepository(ApplicationDbContext db) :base(db)
        {
            this.db = db;
        }
        public void Update(Company company)
        {
            db.Companies.Update(company);
        }
    }
}
