namespace ReportManagementAPI.Repositories.DataModels;

public class Report : BaseEntity
{
    public ReportState ReportState { get; set; }
    public string Path { get; set; }
}

public enum ReportState
{
    Requested,
    InProgress,
    Done
}