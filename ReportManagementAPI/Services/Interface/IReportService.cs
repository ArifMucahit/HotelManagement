using ReportManagementAPI.Models.Dto;

namespace ReportManagementAPI.Services.Interface;

public interface IReportService
{
    Task<List<ReportDto>> GetList();
}