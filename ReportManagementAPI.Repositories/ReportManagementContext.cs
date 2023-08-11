using Microsoft.EntityFrameworkCore;
using ReportManagementAPI.Repositories.DataModels;

namespace ReportManagementAPI.Repositories;

public class ReportManagementContext : DbContext
{
    public ReportManagementContext(DbContextOptions<ReportManagementContext> context) : base(context)
    {
        
    }


    public DbSet<Report> Reports { get; set; }
    
}