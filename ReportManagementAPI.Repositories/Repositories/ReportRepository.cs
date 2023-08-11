using ReportManagementAPI.Repositories.DataModels;
using ReportManagementAPI.Repositories.Repositories.Interface;

namespace ReportManagementAPI.Repositories.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ReportManagementContext context) : base(context)
    {
    }
}