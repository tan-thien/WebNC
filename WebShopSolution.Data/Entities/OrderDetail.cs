using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int IdOrder { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int IdProduct { get; set; }
        public Product Product { get; set; }

        public int SoLuong { get; set; }
        public int Price { get; set; }
    }
}
