using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Voucher
{
    public class VoucherUpdateRequest : VoucherCreateRequest
    {
        public int VoucherId { get; set; }
    }
}
