using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;

namespace HotelManagementAPI.Repositories.Repository;

public class HotelManagerRepository : BaseRepository<HotelManagers>, IHotelManagerRepository
{
    public HotelManagerRepository(HotelManagementContext context) : base(context)
    {
    }
}