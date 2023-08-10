namespace HotelManagementAPI.Models.DTO;

public class ManagerSaveRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid HotelId { get; set; }
}