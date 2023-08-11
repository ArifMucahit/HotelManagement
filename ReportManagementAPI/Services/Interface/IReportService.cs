using ReportManagementAPI.Models.Dto;

namespace ReportManagementAPI.Services.Interface;

public interface IReportService
{
    Task<List<ReportDto>> GetList();
    Task UpdateReport(ReportDto report);
    Task<ReportDto> RequestReport();
}