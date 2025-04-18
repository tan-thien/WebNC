using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerCreateRequest
    {
        [Required]
        public string CusName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int IdAcc { get; set; }
    }

}
