using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Account
{
    public class AccountCreateRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PassWord { get; set; }

        public string? Status { get; set; } = "Active";
        public string Role { get; set; } = "User";
        // Thông tin khách hàng
        [Required]
        public string CusName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
