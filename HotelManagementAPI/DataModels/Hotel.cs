using System.ComponentModel.DataAnnotations;

namespace HotelManagementAPI.DataModels;

public class Hotel : BaseEntity
{
    public Hotel()
    {
        HotelManagers = new HashSet<HotelManagers>();
        ContactInfos = new HashSet<ContactInfo>();
    }
    
    public int Id { get; set; }
    public Guid UUID { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Country { get; set; }
    [MaxLength(255)]
    public string City { get; set; }
    [MaxLength(500)]
    public string FullAddress { get; set; }
    [MaxLength(255)]
    public string CompanyName { get; set; }

    
    
    public virtual HashSet<HotelManagers> HotelManagers { get; set; }
    public virtual HashSet<ContactInfo> ContactInfos { get; set; }
}