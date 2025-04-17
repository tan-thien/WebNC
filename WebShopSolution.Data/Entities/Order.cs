using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }
        public string StatusOrder { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string? MoTa { get; set; }
        public int TongTien { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public string PaymentMethodName { get; set; }

    }
}
