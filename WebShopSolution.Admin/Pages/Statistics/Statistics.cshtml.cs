// Pages/Statistics/Statistics.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShopSolution.Application.Catalog.Statistics;
using WebShopSolution.ViewModels.Catalog.Statistics;

namespace WebShopSolution.Admin.Pages.Statistics
{
    public class StatisticsModel : PageModel
    {
        private readonly IStatisticService _statisticService;

        public StatisticsModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public List<MonthlyOrderStatisticViewModel> Statistics { get; set; }

        public async Task OnGetAsync()
        {
            Statistics = await _statisticService.GetMonthlyOrderStatisticsAsync();
        }

        public async Task<IActionResult> OnGetExportExcelAsync()
        {
            var data = await _statisticService.GetMonthlyOrderStatisticsAsync();
            var content = await _statisticService.ExportStatisticsToExcelAsync(data);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Statistics.xlsx");
        }

        public async Task<IActionResult> OnGetExportPdfAsync()
        {
            var data = await _statisticService.GetMonthlyOrderStatisticsAsync();
            var content = await _statisticService.ExportStatisticsToPdfAsync(data);
            return File(content, "application/pdf", "Statistics.pdf");
        }
    }
}
