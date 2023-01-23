using AutoMapper;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.Domain;
using BookShop.Models.DTO.ProductDTOs;
using BookShop.Models.DTO.ShoppingCartDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopWeb.Controllers
{
    [Route("shoppingcarts")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;        

        public ShoppingCartsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;            
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingCartByUser(string applicationUserId)
        {
            var shoppingCart = await unitOfWork.ShoppingCart.GetAllAsync(sc => sc.ApplicationUserId == applicationUserId, includeProperties: "Product");
            if(shoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCartByUser = new List<ShoppingCartProductsDetailsDTO>();
            double totalPrice = 0;

            foreach (ShoppingCart cart in shoppingCart)
            {
                var priceToShow = GetPrice(cart.ProductsCount, cart.Product!.Price, cart.Product.Price50, cart.Product.Price100);

                var shoppingCartProductDetailsDTO = new ShoppingCartProductsDetailsDTO
                {
                    Title = cart.Product.Title,
                    Count = cart.ProductsCount,
                    Price = priceToShow,
                    ChosenProductsPrice = priceToShow * cart.ProductsCount,
                    ImageUrl = cart.Product.ImageUrl
                };
                shoppingCartByUser.Add(shoppingCartProductDetailsDTO);
                totalPrice += (double)shoppingCartProductDetailsDTO.ChosenProductsPrice;
            };

            return Ok(new ShoppingCartByUserResponseDTO
            { 
                ListShoppingCart = shoppingCartByUser,
                TotalPrice= totalPrice
            });
        }

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetOfferedProductDetails(int productId)
        {
            var productDB = await unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == productId, includeProperties: "Category, CoverType");
            if(productDB == null)
            {
                return NotFound();
            }

            var productDetailsDTO = new OfferedProductDetailsDTO
            {
                Title = productDB.Title,
                Description = productDB.Description,
                ISBN = productDB.ISBN,
                Author = productDB.Author,
                ListPrice = productDB.ListPrice,
                Price = productDB.Price,
                Price50 = productDB.Price50,
                Price100 = productDB.Price100,
                ImageUrl = productDB.ImageUrl,
                Category = productDB.Category!.Name,
                CoverType = productDB.CoverType!.Name,
                Count = 1
            };            

            return Ok(productDetailsDTO); 
        }

        [HttpPost]        
        public async Task<IActionResult> AddItemToShoppingCart(AddItemToShoppingCartDTO item)
        {
            var productDb = await unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == item.ProductId);

            var cartDb = await unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(            
                sc => sc.ApplicationUserId == item.UserId && sc.ProductId == item.ProductId); 

            if (cartDb == null)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    ProductId = item.ProductId,
                    ProductsCount = item.Count,
                    ApplicationUserId = item.UserId,
                    Product = productDb,
                };
                unitOfWork.ShoppingCart.Add(cart);
                await unitOfWork.SaveAsync();
                return Ok(cart.ProductsCount);
            }
            else
            {
                var newCount = unitOfWork.ShoppingCart.IncrementCount(cartDb, item.Count);
                await unitOfWork.SaveAsync();
                return Ok(newCount);
            } 
        }

        [HttpPut]
        [Route("count_increment")]
        public async Task<IActionResult> CountIncrementByOne(int catrId)
        {
            var cart = await unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(c => c.Id == catrId);
            if (cart == null)
            {
                return BadRequest();
            }
            var incrementedProductsCount = unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            await unitOfWork.SaveAsync();
            return Ok(incrementedProductsCount);
        }

        [HttpPut]
        [Route("count_decrement")]

        public async Task<IActionResult> CountDecrementByOne(int cartId)
        {            
            var cart = await unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(c => c.Id == cartId);            
            if (cart == null)
            {
                return BadRequest();
            }

            var decrementedProductsCount = 0;

            if (cart.ProductsCount == 1)
            {
                unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                decrementedProductsCount = unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            
            await unitOfWork.SaveAsync();
            return Ok(decrementedProductsCount);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var cart = await unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
            {
                return BadRequest();
            }
            unitOfWork.ShoppingCart.Remove(cart);
            await unitOfWork.SaveAsync();
            return Ok();
        }        

        private double GetPrice(int count, double price, double price50, double price100)
        {
            if(count < 50)
            {
                return price;
            }
            else
            {
                if(count >= 50 && count > 100)
                {
                    return price50;
                }
                return price100;
            }
        }
    }
}
