namespace HotelManagementAPI.Models.DTO;

public class HotelDTO
{
    public Guid UUID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string FullAddress { get; set; }
    public string CompanyName { get; set; }
}