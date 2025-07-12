using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Statistics;

namespace WebShopSolution.Application.Catalog.Statistics
{
    public interface IStatisticService
    {
        Task<List<MonthlyOrderStatisticViewModel>> GetMonthlyOrderStatisticsAsync();
        Task<byte[]> ExportStatisticsToExcelAsync(List<MonthlyOrderStatisticViewModel> data);
        Task<byte[]> ExportStatisticsToPdfAsync(List<MonthlyOrderStatisticViewModel> data);


        Task<List<int>> GetAvailableOrderYearsAsync();
        Task<List<YearlySummaryViewModel>> GetYearlyStatisticsAsync();
        Task<byte[]> ExportYearlyStatisticsToExcelAsync(List<YearlySummaryViewModel> data);
        Task<byte[]> ExportYearlyStatisticsToPdfAsync(List<YearlySummaryViewModel> data);


        Task<List<DailyStatisticViewModel>> GetStatisticsByDateAsync(DateTime date);
        Task<byte[]> ExportDailyStatisticsToExcelAsync(List<DailyStatisticViewModel> data, DateTime date);
        Task<byte[]> ExportDailyStatisticsToPdfAsync(List<DailyStatisticViewModel> data, DateTime date);


    }
}
