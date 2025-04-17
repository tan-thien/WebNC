using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Customer
    {
        [Key]
        public int IdCus { get; set; }

        public string CusName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [ForeignKey("Account")]
        public int IdAcc { get; set; }
        public Account? Account { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
