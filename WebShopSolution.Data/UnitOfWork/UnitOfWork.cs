using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Repositories.Accounts;
using WebShopSolution.Data.Repositories.Categories;
using WebShopSolution.Data.Repositories.Customers;
using WebShopSolution.Data.Repositories.Orders;
using WebShopSolution.Data.Repositories.Products;

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

        public UnitOfWork(
            WebShopDbContext context,
            IAccountRepository accounts,
            ICustomerRepository customers,
            ICategoryRepository categories,
            IProductRepository products,
            IOrderRepository orders)
        {
            _context = context;
            Accounts = accounts;
            Customers = customers;
            Categories = categories;
            Products = products;
            Orders = orders;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
