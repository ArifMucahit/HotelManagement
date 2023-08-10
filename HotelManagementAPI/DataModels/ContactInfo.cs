namespace HotelManagementAPI.DataModels;

public class ContactInfo : BaseEntity
{

    public int Id { get; set; }
    public Guid UUID { get; set; }
    public ContactType ContactType { get; set; }
    public string ContactAddress { get; set; }
    public int HotelId { get; set; }
}

public enum ContactType
{
    None,
    Email,
    Phone,
    Pigeon,
    Smoke
}