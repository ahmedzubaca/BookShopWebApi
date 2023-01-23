﻿using BookShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId);
    }
}
