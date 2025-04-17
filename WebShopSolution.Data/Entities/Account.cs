using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.Data.Entities
{
    public class Account
    {
        [Key]
        public int IdAcc { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PassWord { get; set; }

        public string? Status { get; set; }

        public string Role { get; set; } = "User";

        // Quan hệ 1-1 với Customer
        public Customer? Customer { get; set; }


    }
}
