using System.Text;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueConsumer;

public class Worker : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private IBasicProperties _queueProperty;
    private HttpClient _httpClient;
    private string _hotelApi;
    private string _reportApi;

    public Worker(IOptions<RabbitMqConfiguration> config, IConfiguration _configuration)
    {
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
        _reportApi = _configuration.GetSection("thirdParty:ReportApi").Value;
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
        var uuid = Encoding.UTF8.GetString(e.Body.ToArray()).Replace("\"", "");

        string updateRequest = $"{{\"reportState : #STATE#, \"path\": \"#path#\", \"uuid\": {uuid},\" }}";


        var httpContent = new StringContent(updateRequest, Encoding.UTF8);
        await _httpClient.PutAsync(_reportApi + "/UpdateState", httpContent);


        var path = await ReadReportData(uuid);
        if (path == null)
        {
            _channel.BasicNack(e.DeliveryTag, false, false);
            updateRequest = updateRequest.Replace("#STATE#", RecordState.Failed.ToString());
            httpContent = new StringContent(updateRequest, Encoding.UTF8);
            await _httpClient.PutAsync(_reportApi + "/UpdateState", httpContent);
        }
        else
        {
            updateRequest = updateRequest.Replace("#STATE#", RecordState.Done.ToString()).Replace("#path#", path);

            await _httpClient.PutAsync(_reportApi + "/UpdatePath", httpContent);
            _channel.BasicAck(e.DeliveryTag, false);
        }
    }


    private async Task<string> ReadReportData(string uuid)
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

    private enum RecordState
    {
        Requested,
        InProgress,
        Done,
        Failed
    }
}