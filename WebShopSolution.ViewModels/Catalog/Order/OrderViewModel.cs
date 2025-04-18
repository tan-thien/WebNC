using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Order
{
    public class OrderViewModel
    {
        public int IdOrder { get; set; }
        public int IdCus { get; set; }
        public string CusName { get; set; }
        public string StatusOrder { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string? MoTa { get; set; }
        public int TongTien { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
