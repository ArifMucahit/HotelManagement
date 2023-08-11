using System.ComponentModel.DataAnnotations;

namespace ReportManagementAPI.Repositories.DataModels;

public class BaseEntity
{
    [Key]
    public Guid UUID { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}