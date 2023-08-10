namespace HotelManagementAPI.DataModels;

public class ContactInfo : BaseEntity
{
    public ContactType ContactType { get; set; }
    public string ContactAddress { get; set; }
    public Guid HotelId { get; set; }
}

public enum ContactType
{
    None,
    Email,
    Phone,
    Pigeon,
    Smoke
}