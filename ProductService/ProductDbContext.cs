using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Model;

namespace ProductService
{
    public class ProductDbContext: DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
               
        }
       public DbSet<Products>Products { get; set; }
    }
}
