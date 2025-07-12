using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShopSolution.Application.Catalog.Statistics;
using WebShopSolution.ViewModels.Catalog.Statistics;

namespace WebShopSolution.Admin.Pages.Statistics
{
    public class StatisticsByDateModel : PageModel
    {
        private readonly IStatisticService _statisticService;

        public StatisticsByDateModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public List<DailyStatisticViewModel> Statistics { get; set; }

        public async Task OnGetAsync()
        {
            Statistics = await _statisticService.GetStatisticsByDateAsync(SelectedDate);
        }

        public async Task<IActionResult> OnGetExportExcelAsync(DateTime selectedDate)
        {
            var data = await _statisticService.GetStatisticsByDateAsync(selectedDate);
            var content = await _statisticService.ExportDailyStatisticsToExcelAsync(data, selectedDate);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"DailyStatistics_{selectedDate:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> OnGetExportPdfAsync(DateTime selectedDate)
        {
            var data = await _statisticService.GetStatisticsByDateAsync(selectedDate);
            var content = await _statisticService.ExportDailyStatisticsToPdfAsync(data, selectedDate);
            return File(content, "application/pdf", $"DailyStatistics_{selectedDate:yyyyMMdd}.pdf");
        }
    }
}
