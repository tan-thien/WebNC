using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Account
{
    public class AccountUpdateRequest
    {
        [Required]
        public int IdAcc { get; set; }

        public string? Status { get; set; }
        public string Role { get; set; } = "User";
    }

}
