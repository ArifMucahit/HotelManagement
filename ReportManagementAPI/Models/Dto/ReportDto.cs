using ReportManagementAPI.Repositories.DataModels;

namespace ReportManagementAPI.Models.Dto;

public class ReportDto
{
    public ReportState ReportState { get; set; }

    public string ReportStateStr
    {
        get { return this.ReportState.ToString(); }
    }

    public string Path { get; set; }
    public Guid UUID { get; set; }
    public DateTime CreatedDate { get; set; }
}