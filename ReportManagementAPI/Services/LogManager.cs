using Microsoft.Extensions.Options;
using OpenSearch.Client;
using ReportManagementAPI.Models;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Services;

public class LogManager : ILogManager
{
    private readonly OpenSearchClient _client;
    private readonly ElasticsearchOptions _config;
    public LogManager(IOptions<ElasticsearchOptions> config)
    {
        _config = config.Value;
        var settings = new ConnectionSettings(new Uri(_config.Uri));
        settings.DisableDirectStreaming();
        settings.BasicAuthentication(_config.Username, _config.Password);
        settings.EnableDebugMode();
        var client = new OpenSearchClient(settings);
        _client = client;
    }

    public async Task LogError(ExceptionLogDto ex)
    {
        await _client.IndexAsync(ex, x => x.Index("exceptionlogs"));
    }
}