namespace ReportManagementAPI.Services.Interface;

public interface IQueueService
{
    void PushQueue(object request);
}