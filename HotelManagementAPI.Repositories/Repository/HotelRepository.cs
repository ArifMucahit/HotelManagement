using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;

namespace HotelManagementAPI.Repositories.Repository;

public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelManagementContext context) : base(context)
    { 
    }
}