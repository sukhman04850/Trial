using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using OrderService.KafkaConsumer;
using OrderService.Model;
using System.Reflection.Metadata.Ecma335;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderInterface _orderFace;
        

        public OrderController(IOrderInterface orderFace)
        {
            _orderFace = orderFace;
            
        }
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderFace.GetAllOrders();
            if (orders == null)
            {
                return NotFound("No Orders are present");
            }
            
            return Ok(orders);  
        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var orders = await _orderFace.GetOrderById(id);
                if(orders== null)
            {
                return NotFound("No order with such id exists");
            }
                return Ok(orders);
        }
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] Orders orderreq)
        {
            var order = await _orderFace.AddOrder(orderreq);

            return Ok(order);
        }
        [HttpDelete("DeleteOrder/{id}")]
        public async Task DeleteOrder(int id)
        {
            await _orderFace.DeleteOrder(id);

        }
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody]Orders order)
        {
            if (order == null)
            {
                return BadRequest("Input is  NUll");
            }
            var newOrder = await _orderFace.UpdateOrder(order);
            
            return Ok(newOrder);

        }



    }
}



