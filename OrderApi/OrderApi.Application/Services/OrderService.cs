using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly.Registry;

using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService
        (
        HttpClient httpClient,
        ResiliencePipelineProvider <string> resiliencePipeline,
        IOrder orderInterface
        ) : IOrderService
    {
        // GET PRODUCT
        public async Task<ProductDTO> GetProduct(int productID)
        {
            //call product api using http client
            //redirect this call to the api gateway since product api does not respond to outsiders
            var getProduct = await httpClient.GetAsync($"/api/products/{productID}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;

        }

        // GET USER
        public async Task<AppUserDTO> GetUser(int userID)
        {
            var getUser = await httpClient.GetAsync($"api/auth/{userID}");
            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        // GET ORDER DETAILS BY ID
        public async Task<OrderDetailDTO> GetOrderDetails(int orderID)
        {
            //prepare order -> OrderDTO
            var order = await orderInterface.FindByIDAsync(orderID);
            if(order is null)
            {
                return null!;
            }
            
            //get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            //prepare product -> ProductDTO
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductID));

            //prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientID));

            //prepare OrderDetails
            return new OrderDetailDTO(
                order.ID,
                productDTO.ID,
                appUserDTO.ID,
                appUserDTO.Email,
                appUserDTO.Name,
                productDTO.Name,
                appUserDTO.Address,
                appUserDTO.Telephone,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Price*productDTO.Quantity,
                order.OrderDate
                );

        }

        // GET ORDER DETAILS BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientID(int clientID)
        {
            //get all client orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientID == clientID);
            if (!orders.Any()) return null!;

            // else convert from entity to DTO

            var (_, _orders) = OrderConversions.FromEntity(null, orders);

            return _orders!;
        }

        public Task<OrderDetailDTO> GetOrdersDetails(int orderID)
        {
            throw new NotImplementedException();
        }
    }
}
