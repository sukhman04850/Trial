using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductService.Interface;
using ProductService.Model;

namespace ProductService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }
        public async Task<Products> AddProduct(Products product)
        {
             await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Products>> GetAllProducts()
        {
            var product = await _context.Products.ToListAsync();
            return product;
        }

        public async Task<Products> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product== null)
            {
                throw new Exception();
            }
            return product;
        }

        public async Task<Products> UpdateProduct(Products product)
        {
            var existing = await _context.Products.FindAsync(product.ProductID);
            if(existing!=null)
            {
                existing.ProductName = product.ProductName;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                _context.Entry(existing).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                throw new Exception("NotFound");
            }
        }
    }
}
