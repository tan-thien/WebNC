using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WebShopSolution.Application.Catalog.Chat;

namespace WebShopSolution.WebApp.Controllers
{

    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }
        [Route("Chat/Ask")]
        [HttpPost]
        public IActionResult Ask([FromBody] ChatRequest request)
        {
            var message = request.Message?.ToLower();

            string reply;

            if (string.IsNullOrEmpty(message))
            {
                reply = "Bạn vui lòng nhập nội dung cần hỗ trợ.";
            }
            else if (message.Contains("mở cửa") || message.Contains("giờ làm việc"))
            {
                reply = "Shop LaLuna mở cửa từ 9h sáng đến 9h tối mỗi ngày.";
            }
            else if (message.Contains("chính sách đổi trả"))
            {
                reply = "Bạn có thể đổi trả trong vòng 7 ngày với hóa đơn mua hàng và sản phẩm chưa qua sử dụng.";
            }
            else if (message.Contains("size") || message.Contains("kích thước"))
            {
                reply = "Bạn vui lòng cho biết sản phẩm nào để mình hỗ trợ về size nhé!";
            }
            else
            {
                reply = "Cảm ơn bạn đã nhắn tin. Nhân viên sẽ hỗ trợ bạn sớm nhất!";
            }

            return Ok(new { reply });
        }


        public class ChatRequest
        {
            public string Message { get; set; }
        }
    }

}
