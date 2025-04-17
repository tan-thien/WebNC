using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Account")]
        public int IdAcc { get; set; }
        public Account Account { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }
}
