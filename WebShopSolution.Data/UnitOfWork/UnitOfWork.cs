using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using WebShopSolution.Data.EF;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebShopDbContext _context;

        public IAccountRepository Accounts { get; }
        public ICustomerRepository Customers { get; }
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IProductVariantRepository ProductVariants { get; }
        public IProductImageRepository ProductImages { get; }

        public IVoucherRepository Vouchers { get; }
        public ICustomerVoucherRepository CustomerVouchers { get; } // ✅ Thêm dòng này

        public UnitOfWork(
            WebShopDbContext context,
            IAccountRepository accounts,
            ICustomerRepository customers,
            ICategoryRepository categories,
            IProductRepository products,
            IOrderRepository orders,
            IOrderDetailRepository orderDetails,
            IProductVariantRepository productVariants,
            IProductImageRepository productImages,
            IVoucherRepository vouchers,
            ICustomerVoucherRepository customerVouchers // ✅ Thêm vào constructor
        )
        {
            _context = context;
            Accounts = accounts;
            Customers = customers;
            Categories = categories;
            Products = products;
            Orders = orders;
            OrderDetails = orderDetails;
            ProductVariants = productVariants;
            ProductImages = productImages;
            Vouchers = vouchers;
            CustomerVouchers = customerVouchers; // ✅ Gán instance
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
