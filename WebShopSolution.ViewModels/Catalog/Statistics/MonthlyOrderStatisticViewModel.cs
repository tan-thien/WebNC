using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Statistics
{
    public class MonthlyOrderStatisticViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalOrders { get; set; }
        public int TotalRevenue { get; set; } // Tổng tiền đơn hàng
    }
    public class MonthlyOrderStatisticFilter
    {
        public int? Year { get; set; } // Năm cần lọc
        public int? Month { get; set; } // Tháng cần lọc (1-12)
    }

}
