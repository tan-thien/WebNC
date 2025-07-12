using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using WebShopSolution.Data.Repositories.Accounts;
using WebShopSolution.Data.Repositories.Categories;
using WebShopSolution.Data.Repositories.Customers;
using WebShopSolution.Data.Repositories.Orders;
using WebShopSolution.Data.Repositories.Products;
using WebShopSolution.Data.Repositories.ProductVariants;
using WebShopSolution.Data.Repositories.ProductImages; 
using WebShopSolution.Data.Repositories.Vouchers;

namespace WebShopSolution.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        ICustomerRepository Customers { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; } 

        IProductVariantRepository ProductVariants { get; }
        IProductImageRepository ProductImages { get; }
        IVoucherRepository Vouchers { get; }
        ICustomerVoucherRepository CustomerVouchers { get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
