using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductService.Interface;
using ProductService.KafkaProducer;
using ProductService.Model;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly KafkaProducers _producer;
        public ProductController(IProductRepository repository,KafkaProducers producer)
        {
            _repository = repository;
            _producer = producer;
        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllProducts();
            if(products == null)
            {
                return NotFound("No Products present");
            }
            return Ok(products);
        }
        [HttpGet("GetProductbyId/{id}")]
        public async Task<IActionResult> GetproductbyId(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product==null)
            {
                return NotFound("Product is not there");
            }
            return Ok(product);
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Products product)
        {
            var productnew = await _repository.AddProduct(product);
            return Ok(productnew);
        }
        [HttpPost("AddOrder/{id}")]
        public async Task<IActionResult> AddOrder(int id)
        {
            var product = await _repository.GetProductById(id);
            Console.WriteLine(product);
            await _producer.Message(id.ToString(),product.ProductID, product.Price, product.Quantity );
            return Ok(product);
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task DeleteProduct([FromRoute]int id)
        {
            await _repository.DeleteProduct(id);
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Products product)
        {
            if (product == null)
            {
                return BadRequest("Data is null");
            }
            var update = await _repository.UpdateProduct(product);
            if (update != null)
            {
                return Ok(update);
            }
            else
            {
                return BadRequest();
            }
        }
       

    }
}
