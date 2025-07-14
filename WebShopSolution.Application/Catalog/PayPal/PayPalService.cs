using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.Entities;
using WebShopSolution.ViewModels.Catalog.Order;
using PayPalCheckoutSdk.Core;
using PayPalOrder = PayPalCheckoutSdk.Orders.Order;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

namespace WebShopSolution.Application.Catalog.PayPal
{
    public class PayPalService
    {
        private readonly IConfiguration _configuration;

        public PayPalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private PayPalEnvironment GetEnvironment()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var secret = _configuration["PayPal:Secret"];
            var mode = _configuration["PayPal:Mode"];

            return mode == "live"
                ? new LiveEnvironment(clientId, secret)
                : new SandboxEnvironment(clientId, secret);
        }

        private PayPalHttpClient GetClient()
        {
            return new PayPalHttpClient(GetEnvironment());
        }

        public async Task<string?> CreateOrder(decimal totalAmount)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
        {
            new PurchaseUnitRequest
            {
                AmountWithBreakdown = new AmountWithBreakdown
                {
                    CurrencyCode = "USD",
                    Value = totalAmount.ToString("F2")
                }
            }
        },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = "https://localhost:7153/Order/PaypalSuccess",
                    CancelUrl = "https://localhost:7153/Order/Checkout"
                }
            });

            var response = await GetClient().Execute(request);
            var result = response.Result<PayPalOrder>();

            return result.Links?.FirstOrDefault(link => link.Rel == "approve")?.Href;
        }


        public async Task<bool> CaptureOrder(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            var response = await GetClient().Execute(request);

            return response.StatusCode == HttpStatusCode.Created;
        }
    }
}
