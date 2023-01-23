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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext db;

        public OrderDetailRepository(ApplicationDbContext db) :base(db) 
        {
            this.db = db;
        }
        public void Update(OrderDetail orderDetail)
        {
            db.OrderDetails.Update(orderDetail);
        }
    }
}
