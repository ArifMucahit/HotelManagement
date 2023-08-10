using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Services.Interface;

public interface IContactService
{
    Task<ContactDto> AddContactInfoAsync(ContactSaveRequest contact, CancellationToken ct = default);
    Task RemoveContactInfoAsync(string id);
}