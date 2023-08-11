using ReportManagementAPI.Models.Dto;
using ReportManagementAPI.Repositories.DataModels;

namespace ReportManagementAPI.Services.Interface;

public interface IReportService
{
    Task<List<ReportDto>> GetList();
    Task UpdateReportPath(string id, string path);
    Task UpdateReportState(string id, ReportState state);
}