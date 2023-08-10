using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Services.Interface;

public interface IHotelService
{
    Task<HotelDTO> AddHotelAsync(HotelSaveRequest hotel, CancellationToken ct = default);
    Task<HotelDTO> GetHotelByIDAsync(string id, CancellationToken ct = default);
    Task RemoveHotelAsync(string id, CancellationToken ct = default);
    Task<List<HotelDTO>> GetHotelList(int pageNumber, int pageSize, CancellationToken ct = default);
}