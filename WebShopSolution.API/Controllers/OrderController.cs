using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebShopSolution.Application.Catalog.Orders;
using WebShopSolution.ViewModels.Catalog.Order;

namespace WebShopSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // API: GET api/order/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // API: GET api/order/user/5 (Xem đơn hàng theo user)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // ✅ API: GET api/order/{id} (Xem chi tiết đơn hàng)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Không tìm thấy đơn hàng với mã {id}");
            return Ok(order);
        }

        // ✅ API: PUT api/order/updatestatus (Cập nhật trạng thái đơn hàng)
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderStatusUpdateRequest request)
        {
            if (request == null || request.OrderId <= 0 || string.IsNullOrEmpty(request.NewStatus))
                return BadRequest("Dữ liệu không hợp lệ");

            var result = await _orderService.UpdateOrderStatusAsync(request);
            if (!result)
                return BadRequest("Cập nhật trạng thái đơn hàng thất bại");

            return Ok("Cập nhật trạng thái thành công");
        }
    }
}
