using ProductService.Model;

namespace ProductService.Interface
{
    public interface IProductRepository
    {
         Task<List<Products>> GetAllProducts();
        Task<Products> GetProductById(int id);
        Task<Products> AddProduct(Products product);
        Task<Products> UpdateProduct(Products product);
        Task DeleteProduct(int id);


    }
}
