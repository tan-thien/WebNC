using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductVariantproductstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");
        }
    }
}
