namespace HotelManagementAPI.DataModels;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public bool isDeleted { get; set; }
}