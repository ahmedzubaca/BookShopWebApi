using BookShop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookShop.Models.DTO.ProductDTOs;
using AutoMapper;
using BookShop.Models.Domain;

namespace BookShopWeb.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var productsDb = await unitOfWork.Product.GetAllAsync(includeProperties: "Category, CoverType");            
            if(productsDb == null)
            {
                return NotFound();
            }
            var productsDTO = mapper.Map<List<ProductDTO>>(productsDb);
            var getAllProductsResponse = new GetAllProductsResponse { Products = productsDTO };

            return Ok(getAllProductsResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute]int id)
        {
            var productDb = await unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == id, includeProperties: "Category, CoverType");
            if(productDb == null)
            {
                return NotFound();
            }
            var productDTO = mapper.Map<ProductDTO>(productDb);
            var getSingleProductResponse = new GetSingleProductResponse { Product = productDTO };

            return Ok(getSingleProductResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody]AddProductRequest addProdactReq)
        {
            if(addProdactReq.Product == null)
            {
                return BadRequest();
            }

            var addProduct = addProdactReq.Product;

            var productToAdd = new Product
            {
                Title = addProduct.Title,
                Description = addProduct.Description,
                Author = addProduct.Author,
                ISBN = addProduct.ISBN,
                ListPrice = addProduct.ListPrice,
                Price = addProduct.Price,
                Price50 = addProduct.Price50,
                Price100 = addProduct.Price100,
                ImageUrl = addProduct.ImageUrl,
                CategoryId = addProduct.CategoryId,
                CoverTypeId = addProduct.CoverTypeId,
            };
            unitOfWork.Product.Add(productToAdd);
            await unitOfWork.SaveAsync();

            var productDTO = mapper.Map<ProductDTO>(productToAdd);
            var singleProdactResponse = new GetSingleProductResponse { Product = productDTO };

            return CreatedAtAction("GetProductById", new {id = singleProdactResponse.Product.Id}, singleProdactResponse);            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            if(request.Product == null)
            {
                return BadRequest();
            }

            var productDB = await unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id);
            if(productDB == null)
            {
                return BadRequest();
            } 
            
            var reqProduct = request.Product;           

            if (reqProduct.Title != null) productDB.Title = reqProduct.Title;
            if (reqProduct.Description != null) productDB.Description = reqProduct.Description;
            if (reqProduct.Author != null) productDB.Author = reqProduct.Author;
            if (reqProduct.ISBN != null)  productDB.ISBN = reqProduct.ISBN;
            if (reqProduct.ListPrice != null) productDB.ListPrice = (double)reqProduct.ListPrice;
            if (reqProduct.Price != null) productDB.Price = (double)reqProduct.Price;
            if (reqProduct.Price50 != null) productDB.Price50 = (double)reqProduct.Price50;
            if (reqProduct.Price100 != null) productDB.Price100 = (double)reqProduct.Price100;
            if (reqProduct.ImageUrl != null) productDB.ImageUrl = reqProduct.ImageUrl;
            if (reqProduct.CategoryId != null) productDB.CategoryId = (int)reqProduct.CategoryId;
            if (reqProduct.CoverTypeId != null) productDB.CoverTypeId = (int)reqProduct.CoverTypeId; 
            
            unitOfWork.Product.Update(productDB);
            await unitOfWork.SaveAsync();

            var productDTO = mapper.Map<ProductDTO>(productDB);
            var getSingleProductResponse = new GetSingleProductResponse { Product = productDTO };

            return Ok(getSingleProductResponse);            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var productDb = await unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == id);
            if (productDb == null)
            {
                return BadRequest();
            }

            unitOfWork.Product.Remove(productDb);
            await unitOfWork.SaveAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("deleterange")]
        public async Task<IActionResult> DeleteRange(List<Product> productsList)
        {
            unitOfWork.Product.RemoveRange(productsList);
            await unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
