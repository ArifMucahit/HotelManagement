using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Services.Interface;

public interface ILogManager
{
    Task LogError(ExceptionLogDto ex);
}