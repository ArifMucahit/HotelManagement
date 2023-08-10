using System.ComponentModel.DataAnnotations;

namespace HotelManagementAPI.DataModels;

public class HotelManagers : BaseEntity
{
    public Guid UUID { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Surname { get; set; }
    
    public int HotelId { get; set; }
}
