using System.Text;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportManagementAPI.Models;
using ReportManagementAPI.Repositories.DataModels;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Services;

public class QueueConsumer : BackgroundService
{
    private readonly IReportService _reportService;
    private IConnection _connection;
    private IModel _channel;
    private IBasicProperties _queueProperty;
    private HttpClient _httpClient;
    private string _hotelApi;

    
    public QueueConsumer(IOptions<RabbitMqConfiguration> config, IReportService reportService, IConfiguration _configuration)
    {
        _reportService = reportService;
        _httpClient = new HttpClient();
        var rabbitConfig = config.Value;
        var connection = new ConnectionFactory()
        {
            HostName = rabbitConfig.Host,
            UserName = rabbitConfig.Username,
            Password = rabbitConfig.Password,
            AutomaticRecoveryEnabled = true,
            Port = rabbitConfig.Port,
            VirtualHost = "/"
        };
        _connection = connection.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare("report", true, false, false, null);

        _queueProperty = _channel.CreateBasicProperties();
        _queueProperty.Persistent = true;
        _hotelApi = _configuration.GetSection("thirdParty:HotelApi").Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _channel.BasicQos(0, 1, false);
            var consumerSave = new EventingBasicConsumer(_channel);
            _channel.BasicConsume("report", false, "", consumerSave);

            consumerSave.Received += ConsumerSave_Received;
        }
    }

    private async void ConsumerSave_Received(object? sender, BasicDeliverEventArgs e)
    {
        var uuid = Encoding.UTF8.GetString(e.Body.ToArray()).Replace("\"","");

        await _reportService.UpdateReportState(uuid, ReportState.InProgress);
        var path = await ReadReportData(uuid);
        if (path == null)
        {
            _channel.BasicNack(e.DeliveryTag,false,false);
            await _reportService.UpdateReportState(uuid, ReportState.Failed);
        }
        else
        {
            await _reportService.UpdateReportPath(uuid, path);
            _channel.BasicAck(e.DeliveryTag, false);
        }
    }

    
    private  async Task<string> ReadReportData(string uuid)
    {
        var data = await _httpClient.GetStringAsync(_hotelApi + "/Hotel/GetReport");
        if (data == null)
            return null;
        var path = uuid + ".xlsx";
        var workBook = new Workbook();
        var workSheet = workBook.Worksheets[0];
        var jsonOpt = new JsonLayoutOptions();

        jsonOpt.ArrayAsTable = true;

        JsonUtility.ImportData(data, workSheet.Cells, 0, 0, jsonOpt);

        workBook.Save(path);
        return path;
    }
}