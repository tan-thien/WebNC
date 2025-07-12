using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.Data.EF;
using WebShopSolution.ViewModels.Catalog.Statistics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using DrawingColor = System.Drawing.Color;



namespace WebShopSolution.Application.Catalog.Statistics
{
    public class StatisticService : IStatisticService
    {
        private readonly WebShopDbContext _context;

        public StatisticService(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyOrderStatisticViewModel>> GetMonthlyOrderStatisticsAsync()
        {
            var statistics = await _context.Orders
                .Where(o => o.OrderStatus == "Đã giao") // hoặc loại bỏ nếu muốn thống kê tất cả
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new MonthlyOrderStatisticViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(s => s.Year).ThenByDescending(s => s.Month)
                .ToListAsync();

            return statistics;
        }

        public async Task<byte[]> ExportStatisticsToExcelAsync(List<MonthlyOrderStatisticViewModel> data)
        {
            // Bản EPPlus 7: LicenseContext cần được set như sau
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Statistics");

            // Header
            worksheet.Cells["A1"].Value = "Year";
            worksheet.Cells["B1"].Value = "Month";
            worksheet.Cells["C1"].Value = "Total Orders";
            worksheet.Cells["D1"].Value = "Total Revenue";

            // Dữ liệu
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                worksheet.Cells[i + 2, 1].Value = item.Year;
                worksheet.Cells[i + 2, 2].Value = item.Month;
                worksheet.Cells[i + 2, 3].Value = item.TotalOrders;
                worksheet.Cells[i + 2, 4].Value = item.TotalRevenue;
            }

            worksheet.Cells.AutoFitColumns();

            // Trả về file dạng byte[]
            return await package.GetAsByteArrayAsync();
        }



        public async Task<byte[]> ExportStatisticsToPdfAsync(List<MonthlyOrderStatisticViewModel> data)
        {
            // Khai báo license cho QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Monthly Order Statistics").FontSize(20).Bold().AlignCenter();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Year").Bold();
                            header.Cell().Element(CellStyle).Text("Month").Bold();
                            header.Cell().Element(CellStyle).Text("Total Orders").Bold();
                            header.Cell().Element(CellStyle).Text("Total Revenue").Bold();
                        });

                        // Rows
                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.Year.ToString());
                            table.Cell().Element(CellStyle).Text(item.Month.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalOrders.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalRevenue.ToString("N0") + " ₫");
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                });
            });

            return await Task.FromResult(document.GeneratePdf());
        }



        /*======================================Năm==========================================*/

        public async Task<List<YearlySummaryViewModel>> GetYearlyStatisticsAsync()
        {
            var result = await _context.Orders
                .Where(o => o.OrderStatus == "Đã giao")
                .GroupBy(o => o.OrderDate.Year)
                .Select(g => new YearlySummaryViewModel
                {
                    Year = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.Year)
                .ToListAsync();

            return result;
        }



        public async Task<byte[]> ExportYearlyStatisticsToExcelAsync(List<YearlySummaryViewModel> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Yearly Summary");

            worksheet.Cells["A1"].Value = "Year";
            worksheet.Cells["B1"].Value = "Total Orders";
            worksheet.Cells["C1"].Value = "Total Revenue";

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = data[i].Year;
                worksheet.Cells[i + 2, 2].Value = data[i].TotalOrders;
                worksheet.Cells[i + 2, 3].Value = data[i].TotalRevenue;
            }

            worksheet.Cells.AutoFitColumns();
            return await package.GetAsByteArrayAsync();
        }

        public async Task<byte[]> ExportYearlyStatisticsToPdfAsync(List<YearlySummaryViewModel> data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Yearly Order Statistics").FontSize(20).Bold().AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Year").Bold();
                            header.Cell().Element(CellStyle).Text("Total Orders").Bold();
                            header.Cell().Element(CellStyle).Text("Total Revenue").Bold();
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.Year.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalOrders.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalRevenue.ToString("N0") + " ₫");
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                });
            });

            return await Task.FromResult(document.GeneratePdf());
        }

        public async Task<List<int>> GetAvailableOrderYearsAsync()
        {
            return await _context.Orders
                .Select(o => o.OrderDate.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }



        /*======================================ngày==========================================*/

        public async Task<List<DailyStatisticViewModel>> GetStatisticsByDateAsync(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            var data = await _context.Orders
                .Where(o => o.OrderDate >= start && o.OrderDate < end && o.OrderStatus == "Đã giao")
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new DailyStatisticViewModel
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            return data;
        }

        public async Task<byte[]> ExportDailyStatisticsToExcelAsync(List<DailyStatisticViewModel> data, DateTime date)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Daily Statistics");

            sheet.Cells["A1"].Value = $"Statistics for {date:yyyy-MM-dd}";
            sheet.Cells["A1:C1"].Merge = true;
            sheet.Cells["A1"].Style.Font.Bold = true;
            sheet.Cells["A1"].Style.Font.Size = 16;

            sheet.Cells["A2"].Value = "Date";
            sheet.Cells["B2"].Value = "Total Orders";
            sheet.Cells["C2"].Value = "Total Revenue";

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                sheet.Cells[i + 3, 1].Value = item.Date.ToString("yyyy-MM-dd");
                sheet.Cells[i + 3, 2].Value = item.TotalOrders;
                sheet.Cells[i + 3, 3].Value = item.TotalRevenue;
            }

            sheet.Cells.AutoFitColumns();
            return await package.GetAsByteArrayAsync();
        }

        public async Task<byte[]> ExportDailyStatisticsToPdfAsync(List<DailyStatisticViewModel> data, DateTime date)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Daily Statistics - {date:yyyy-MM-dd}").FontSize(20).Bold().AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Date").Bold();
                            header.Cell().Element(CellStyle).Text("Total Orders").Bold();
                            header.Cell().Element(CellStyle).Text("Total Revenue").Bold();
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.Date.ToString("yyyy-MM-dd"));
                            table.Cell().Element(CellStyle).Text(item.TotalOrders.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalRevenue.ToString("N0") + " ₫");
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                });
            });

            return await Task.FromResult(document.GeneratePdf());
        }




    }

}
