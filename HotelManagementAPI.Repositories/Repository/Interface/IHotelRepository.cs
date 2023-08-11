using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;

namespace HotelManagementAPI.Repositories.Repository.Interface;

public interface IHotelRepository : IBaseRepository<Hotel>
{
    Task<List<ReportDto>> GetHotelReport();
}