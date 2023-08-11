using ReportManagementAPI.Models;

namespace ReportManagementAPI.Services.Interface;

public interface ILogManager
{
    Task LogError(ExceptionLogDto ex);
}