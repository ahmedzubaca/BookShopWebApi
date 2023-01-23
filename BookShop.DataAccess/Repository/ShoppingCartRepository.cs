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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
           shoppingCart.ProductsCount -= count;
            return shoppingCart.ProductsCount;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.ProductsCount += count;
            return shoppingCart.ProductsCount;
        }
    }
}
