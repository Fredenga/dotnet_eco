using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversions
    {
        public static Order ToEntity(OrderDTO order) => new Order()
        {
            ID = order.ID,
            ClientID = order.ClientID,
            ProductID = order.ProductID,
            PurchaseQuantity = order.PurchaseQuantity,
            OrderDate = order.OrderDate
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            // return single
            if (order is not null || orders is null)
            {
                var singleOrder = new OrderDTO(
                   order!.ID,
                   order!.ClientID,
                   order!.ProductID,
                   order!.PurchaseQuantity,
                   order.OrderDate
                );
                return (singleOrder, null);
            }
            if (order is null || orders is not null)
            {
                var _orders = orders!.Select(o => new OrderDTO(
                   o!.ID,
                   o!.ClientID,
                   o!.ProductID,
                   o!.PurchaseQuantity,
                   o.OrderDate
                ));
                return (null, _orders);
            }
            return (null, null);
        }
        
    }
}
