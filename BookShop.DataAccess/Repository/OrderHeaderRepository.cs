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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var orderHeaderDb = db.OrderHeaders.FirstOrDefault(oh => oh.Id == orderId);
            if(orderHeaderDb != null)
            {
                orderHeaderDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderHeaderDb.PaymentStatus = paymentStatus;
                }
            }            
        }

        public void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId)
        {
            var orderHeaderDb = db.OrderHeaders.FirstOrDefault(oh => oh.Id == orderId);
            if (orderHeaderDb != null)
            {
                orderHeaderDb.SessionId = sessionId;
                orderHeaderDb.PaymentIntentId = paymentIntentId;
            }
        }
    }
}
