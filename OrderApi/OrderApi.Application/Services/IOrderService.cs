using OrderApi.Application.DTOs;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientID(int clientID);
        Task<OrderDetailDTO> GetOrdersDetails(int orderID);
    }
}
