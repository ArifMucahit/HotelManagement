namespace HotelManagementAPI.Models.DTO;

public class HotelSaveRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string FullAddress { get; set; }
    public string CompanyName { get; set; }
}