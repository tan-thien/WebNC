using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Order
{
    public class OrderCreateRequest
    {
        [Required]
        public int IdCus { get; set; }

        [Required]
        public string StatusOrder { get; set; }

        public string? DiaChi { get; set; }

        public string? MoTa { get; set; }

        public int TongTien { get; set; }

        [Required]
        public string PaymentMethodName { get; set; }
    }

}
