using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShopSolution.Application.Catalog.Statistics;
using WebShopSolution.ViewModels.Catalog.Statistics;

namespace WebShopSolution.Admin.Pages.Statistics
{
    public class StatisticsByYearModel : PageModel
    {
        private readonly IStatisticService _statisticService;

        public StatisticsByYearModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public List<YearlySummaryViewModel> Statistics { get; set; }

        public async Task OnGetAsync()
        {
            Statistics = await _statisticService.GetYearlyStatisticsAsync();
        }

        public async Task<IActionResult> OnGetExportExcelAsync()
        {
            var data = await _statisticService.GetYearlyStatisticsAsync();
            var content = await _statisticService.ExportYearlyStatisticsToExcelAsync(data);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"YearlyStatistics.xlsx");
        }

        public async Task<IActionResult> OnGetExportPdfAsync()
        {
            var data = await _statisticService.GetYearlyStatisticsAsync();
            var content = await _statisticService.ExportYearlyStatisticsToPdfAsync(data);
            return File(content, "application/pdf", $"YearlyStatistics.pdf");
        }

    }
}
