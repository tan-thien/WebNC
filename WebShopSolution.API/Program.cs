using Microsoft.EntityFrameworkCore;
using WebShopSolution.Application.Catalog.Accounts;
using WebShopSolution.Application.Catalog.Categories;
using WebShopSolution.Application.Catalog.Orders;
using WebShopSolution.Application.Catalog.ProductVariants;
using WebShopSolution.Application.Catalog.Products;
using WebShopSolution.Application.Catalog.Vouchers;
using WebShopSolution.Data.EF;
using WebShopSolution.Data.Repositories.Accounts;
using WebShopSolution.Data.Repositories.Categories;
using WebShopSolution.Data.Repositories.Customers;
using WebShopSolution.Data.Repositories.Orders;
using WebShopSolution.Data.Repositories.Products;
using WebShopSolution.Data.Repositories.ProductVariants;
using WebShopSolution.Data.Repositories.ProductImages;
using WebShopSolution.Data.Repositories.Vouchers;
using WebShopSolution.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------
// 🗄️ DbContext
// ------------------------------------------------------------------
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDb")));

// ------------------------------------------------------------------
// 📥 Repository
// ------------------------------------------------------------------
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICustomerVoucherRepository, CustomerVoucherRepository>(); // ✅ THÊM DÒNG NÀY

// ------------------------------------------------------------------
// 💼 Unit of Work
// ------------------------------------------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ------------------------------------------------------------------
// 🛠️ Service
// ------------------------------------------------------------------
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<IProductVariantAttributeService, ProductVariantAttributeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();

// ------------------------------------------------------------------
// 🌐 Swagger & Controllers
// ------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor(); 
var app = builder.Build();

// ------------------------------------------------------------------
// 🚀 Middleware
// ------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
