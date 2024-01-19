using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using OrderService.Model;

namespace OrderService.Repositories
{
    public class OrderRepository: IOrderInterface
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Orders> AddOrder(Orders orderrequest)
        {
            await _context.Orders.AddAsync(orderrequest);
            await _context.SaveChangesAsync();
            return orderrequest;
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            var allOrders = await _context.Orders.ToListAsync();
            return allOrders;
        }

        public async Task<List<Orders>> GetOrderById(int id)
        {
            var order = await _context.Orders.Where(x => x.OrderID == id).ToListAsync();
            return order;
        }

        public async Task<Orders> UpdateOrder(Orders orderupdate)
        {
            var existing = await _context.Orders.FindAsync(orderupdate.OrderID);
            if (existing != null)
            {
                
                existing.Price = orderupdate.Price;
                existing.Quantity = orderupdate.Quantity;
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
