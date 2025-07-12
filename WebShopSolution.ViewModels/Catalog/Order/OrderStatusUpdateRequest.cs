using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Order
{
    public class OrderStatusUpdateRequest
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
    }
}
