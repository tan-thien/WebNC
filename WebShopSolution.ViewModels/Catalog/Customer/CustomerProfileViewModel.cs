using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerProfileViewModel
    {
        public string CusName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string UserName { get; set; } = "";
        public string PassWord { get; set; } = ""; // Mật khẩu mới (trống mặc định khi Edit)
        public string? CurrentPassword { get; set; } // Mật khẩu hiện tại (chỉ dùng khi xác minh)
    }
}
