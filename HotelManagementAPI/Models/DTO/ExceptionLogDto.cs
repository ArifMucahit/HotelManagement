namespace HotelManagementAPI.Models.DTO;

public class ExceptionLogDto
{
    public Exception Exception { get; set; }
    public DateTime DateTime { get; set; }
    public string Source { get; set; }
}