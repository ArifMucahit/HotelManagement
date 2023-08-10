using HotelManagementAPI.DataModels;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Repositories;

public class HotelManagementContext : DbContext
{
    public HotelManagementContext(DbContextOptions<HotelManagementContext> options) : base(options)
    {
        
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelManagers> HotelManagers { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }
    
}