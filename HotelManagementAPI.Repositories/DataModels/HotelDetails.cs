namespace HotelManagementAPI.DataModels;

public class HotelDetails
{
    public Guid UUID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string FullAddress { get; set; }
    public string CompanyName { get; set; }
    public List<HotelContactInfo> ContactInfos { get; set; }
}

public class HotelContactInfo
{
    public ContactType ContactType { get; set; }

    public string ContactTypeStr
    {
        get
        {
            return this.ContactType.ToString();
        }
    }
    public string ContactAddress { get; set; }
    public Guid UUID { get; set; }
}