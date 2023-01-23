using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            this.Category = new CategoryRepository(db);
            this.CoverType = new CoverTypeRepository(db);
            this.Product = new ProductRepository(db);
            this.Company = new CompanyRepository(db);
            this.ShoppingCart = new ShoppingCartRepository(db);
            this.ApplicationUser = new ApplicationUserRepository(db);
            this.OrderDetail = new OrderDetailRepository(db);
            this.OrderHeader = new OrderHeaderRepository(db);
        }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; } 

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }       
    }
}
