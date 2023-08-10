using HotelManagementAPI.DataModels;

namespace HotelManagementAPI.Models.DTO;

public class ContactSaveRequest
{
    public ContactType ContactType { get; set; }
    public string ContactAddress { get; set; }
    public Guid HotelId { get; set; }
}