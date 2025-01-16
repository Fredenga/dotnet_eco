using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using SharedLib.Responses;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if(!orders.Any()){
                return NotFound("no order detected");
            }

            var (_, list) = OrderConversions.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIDAsync(id);
            if (order == null) {
                return NotFound(null);
            }

            var (_order, _) = OrderConversions.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO order)
        {
            // check model state if all data annotations have been passed
            if (!ModelState.IsValid)
            {
                return BadRequest("incomplete information submitted");
            }
            // convert to entity
            var entity = OrderConversions.ToEntity(order);
            var response = await orderInterface.CreateAsync(entity);
            return response.Flag ? Ok(response) : BadRequest(response);

        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            // convert from DTO to entity
            var order = OrderConversions.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);

            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // convert from DTO to entity
            var order = OrderConversions.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);

            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest("invalid information provided");
            }
            var orders = await orderService.GetOrdersByClientID(clientId);
            return !orders.Any() ? NotFound(null) : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("invalid information provided");
            }
            var orderDetail = await orderService.GetOrdersDetails(orderId);

            return orderDetail.ID > 0 ? Ok(orderDetail) : NotFound("no order found");
        }
    }
}
