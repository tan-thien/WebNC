using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebShopSolution.Application.Catalog.Chat
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ChatService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var apiKey = _config["OpenRouter:ApiKey"]; // ⚠️ Bạn đã lưu API Key trong appsettings.json

            var requestData = new
            {
                model = "mistralai/mistral-7b-instruct",
                messages = new[]
                {
                new { role = "user", content = message }
            },
                temperature = 0.7
            };

            var requestJson = JsonConvert.SerializeObject(requestData);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://openrouter.ai/api/v1/chat/completions"),
                Headers =
            {
                { "Authorization", $"Bearer {apiKey}" },
                { "HTTP-Referer", "https://localhost:7153/" }, // 🔁 Cập nhật nếu bạn dùng app thật
                { "X-Title", "YourApp Chat" } // Tuỳ chọn
            },
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"❌ Lỗi gọi OpenRouter: {response.StatusCode} - {error}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(resultJson);
            string reply = result.choices[0].message.content;
            return reply.Trim();
        }
    }

}
