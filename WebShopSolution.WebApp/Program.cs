using Microsoft.EntityFrameworkCore;
using WebShopSolution.Application.Catalog.Accounts;
using WebShopSolution.Application.Catalog.Carts;
using WebShopSolution.Application.Catalog.Customers;
using WebShopSolution.Application.Catalog.Orders;
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
using WebShopSolution.Application.Catalog.Categories;
using WebShopSolution.Application.Catalog.PayPal;
using WebShopSolution.Application.Catalog.Chat;

var builder = WebApplication.CreateBuilder(args);

// ✅ Đăng ký DbContext
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDb")));

// ✅ Đăng ký các repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICustomerVoucherRepository, CustomerVoucherRepository>(); // ✅ Thêm dòng này
builder.Services.AddScoped<PayPalService>();
builder.Services.AddHttpClient<ChatService>();

// ✅ Đăng ký UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ✅ Đăng ký các service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddHttpClient();


// ✅ Dùng để inject HttpContext vào Service (ví dụ lấy Session)
builder.Services.AddHttpContextAccessor();

// ✅ Đăng ký MVC
builder.Services.AddControllersWithViews();

// ✅ Đăng ký session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ✅ Cấu hình middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // ✅ sử dụng session
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
