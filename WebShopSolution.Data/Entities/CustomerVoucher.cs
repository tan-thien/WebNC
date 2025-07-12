using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSolution.Data.Entities
{
    public class CustomerVoucher
    {
        [Key]
        public int CustomerVoucherId { get; set; }

        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime? UsedAt { get; set; }
    }
}
