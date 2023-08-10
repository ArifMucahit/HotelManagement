using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Services.Interface;

public interface IManagerService
{
    Task<ManagerDto> AddManagerAsync(ManagerSaveRequest manager, CancellationToken ct = default);
    Task<List<ManagerDto>> ListManagersAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}