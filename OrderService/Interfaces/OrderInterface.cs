using OrderService.Model;

namespace OrderService.Interfaces
{
    public interface IOrderInterface
    {
        Task<Orders> AddOrder(Orders orderrequest);
        Task<List<Orders>> GetAllOrders();
        Task<List<Orders>> GetOrderById(int id);
        Task<Orders> UpdateOrder(Orders orderupdate);
        Task DeleteOrder(int id);

    }
}
