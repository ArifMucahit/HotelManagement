using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ReportManagementAPI.Models;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Services;

public class QueueService : IQueueService
{
    private IConnection _connection = null;
    private IModel _channel = null;
    private IBasicProperties _queueProperty;

    public QueueService(IOptions<RabbitMqConfiguration> config)
    {
        var rabbitConfig = config.Value;
        var connection = new ConnectionFactory()
        {
            HostName = rabbitConfig.Host,
            UserName = rabbitConfig.Username,
            Password = rabbitConfig.Password,
            AutomaticRecoveryEnabled = true,
            Port = rabbitConfig.Port,
            VirtualHost  = "/"
        };
        _connection = connection.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare("report", true, false, false, null);

        _queueProperty = _channel.CreateBasicProperties();
        _queueProperty.Persistent = true;
    }
    
    public void PushQueue(string request)
    {
        var stringModel = JsonSerializer.Serialize(request);
        var byteModel = Encoding.UTF8.GetBytes(stringModel);
        _channel.BasicPublish(string.Empty, "report", false, _queueProperty, byteModel);
    }
}