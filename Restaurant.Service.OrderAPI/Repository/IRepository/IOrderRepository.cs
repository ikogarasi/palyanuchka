using Restaurant.Service.OrderAPI.Models;

namespace Restaurant.Service.OrderAPI.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader order);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
