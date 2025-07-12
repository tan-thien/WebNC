using Microsoft.EntityFrameworkCore;

using WebShopSolution.Data.EF;
using WebShopSolution.Data.Repositories.Accounts;
using WebShopSolution.Data.Repositories.Categories;
using WebShopSolution.Data.Repositories.Customers;
using WebShopSolution.Data.Repositories.Orders;
using WebShopSolution.Data.Repositories.ProductImages;
using WebShopSolution.Data.Repositories.Products;
using WebShopSolution.Data.Repositories.ProductVariants;
using WebShopSolution.Data.Repositories.Vouchers;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.Application.Catalog.Statistics;
using WebShopSolution.Application.Catalog.Accounts;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------
// 🛠 Add services to the container
// ------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // dùng để gọi API từ AdminController

// ✅ Session config
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Repository đăng ký
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICustomerVoucherRepository, CustomerVoucherRepository>(); // ✅ THÊM dòng này
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// ✅ Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ✅ DbContext
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDb")));

builder.Services.AddRazorPages();

// ------------------------------------------------------------------
// 🚀 Middleware
// ------------------------------------------------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages(); // 🔥 bắt buộc
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=AdminAuth}/{action=Login}/{id?}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminAuth}/{action=Login}/{id?}");

app.Run();
