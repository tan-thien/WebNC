using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using WebShopSolution.Data.EF;

public class WebShopDbContextFactory : IDesignTimeDbContextFactory<WebShopDbContext>
{
    public WebShopDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebShopDbContext>();

        // Đảm bảo đúng thư mục và đọc tệp appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Đảm bảo đúng thư mục
            .AddJsonFile("appsettings.json") // Đọc cấu hình từ appsettings.json
            .Build();

        // Lấy chuỗi kết nối từ appsettings.json
        var connectionString = configuration.GetConnectionString("WebShopDb");
        optionsBuilder.UseSqlServer(connectionString);

        return new WebShopDbContext(optionsBuilder.Options);
    }
}
