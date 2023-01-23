using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.Domain;
using BookShop.Models.DTO.OrderDTOs;
using BookShop.Models.DTO.ShoppingCartDTOs;
using BookShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;

namespace BookShopWeb.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("order_summary")]
        public async Task<IActionResult> OrderSummary(string aplicationUserId)
        {
            var shoppingCartDb = await unitOfWork.ShoppingCart.GetAllAsync(
                sc => sc.ApplicationUserId == aplicationUserId, includeProperties: "Product");

            var productsSummaryByUser = new List<productsSummaryByIdDTO>();
            double totalPrice = 0;

            foreach (ShoppingCart cart in shoppingCartDb)
            {
                var priceToShow = GetPrice(cart.ProductsCount, cart.Product!.Price, cart.Product.Price50, cart.Product.Price100);

                var productDetails = new productsSummaryByIdDTO
                {
                    Title = cart.Product.Title,
                    Count = cart.ProductsCount,
                    Price = priceToShow,
                    ChosenProductsPrice = priceToShow * cart.ProductsCount                    
                };
                productsSummaryByUser.Add(productDetails);
                totalPrice += productDetails.ChosenProductsPrice;
            };

            var orderSummaryResponse = new OrderSummaryResponseDTO
            {
                ProductsSummaryDTOs = productsSummaryByUser,
                ShippingDetails = new()
            };

            OrderHeader orderHeader = new();
            orderHeader.ApplicationUser = await unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(
                u => u.Id == aplicationUserId);
            orderSummaryResponse.ShippingDetails.Name = orderHeader.ApplicationUser.Name;
            orderSummaryResponse.ShippingDetails.PhoneNumber = orderHeader.ApplicationUser.PhoneNumber;
            orderSummaryResponse.ShippingDetails.StreetAddress = orderHeader.ApplicationUser.StreetAddress;
            orderSummaryResponse.ShippingDetails.City = orderHeader.ApplicationUser.City;
            orderSummaryResponse.ShippingDetails.State = orderHeader.ApplicationUser.State;
            orderSummaryResponse.ShippingDetails.PostalCode = orderHeader.ApplicationUser.PostalCode;
            orderSummaryResponse.ShippingDetails.OrderTotal = totalPrice;          

            return Ok(orderSummaryResponse);
        }

        [HttpPost]        
        public async Task<IActionResult> PostOrder(string applicationUser)
        {
            var listCart = await unitOfWork.ShoppingCart.GetAllAsync(
                sc => sc.ApplicationUserId == applicationUser, includeProperties: "Product");

            var orderHeader = new OrderHeader();
            double price = 0;

            orderHeader.PaymentStatus = SD.PaymentStatusPending;
            orderHeader.OrderStatus = SD.StatusPending;
            orderHeader.OrderDate = DateTime.Now;
            orderHeader.ApplicationUserId = applicationUser;

            foreach(var cart in listCart)
            {
                price = GetPrice(cart.ProductsCount, cart.Product!.Price, 
                    cart.Product.Price50, cart.Product.Price100);
                orderHeader.OrderTotal += (price * cart.ProductsCount);
            }

            orderHeader.ApplicationUser = await unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u=> u.Id == applicationUser);

            orderHeader.Name = orderHeader.ApplicationUser.Name;
            orderHeader.PhoneNumber = orderHeader.ApplicationUser.PhoneNumber;
            orderHeader.StreetAddress = orderHeader.ApplicationUser.StreetAddress;
            orderHeader.City = orderHeader.ApplicationUser.City;
            orderHeader.State = orderHeader.ApplicationUser.State;
            orderHeader.PostalCode = orderHeader.ApplicationUser.PostalCode;            

            unitOfWork.OrderHeader.Add(orderHeader);
            await unitOfWork.SaveAsync();

            var orderDetail = listCart.Select(cart => new OrderDetail
            {
                ProductId = cart.ProductId,
                OrderId = orderHeader.Id,
                Price = price,
                ProductCount = cart.ProductsCount,
            }).ToList();

            unitOfWork.OrderDetail.AddRange(orderDetail);
            await unitOfWork.SaveAsync();

            unitOfWork.ShoppingCart.RemoveRange(listCart);
            await unitOfWork.SaveAsync();
            return Ok();       
        }        

        private double GetPrice(int count, double price, double price50, double price100)
        {
            if (count < 50)
            {
                return price;
            }
            else
            {
                if (count >= 50 && count > 100)
                {
                    return price50;
                }
                return price100;
            }
        }
    }
}
