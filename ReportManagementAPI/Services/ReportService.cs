using System.Dynamic;
using AutoMapper;
using ReportManagementAPI.Models.Dto;
using ReportManagementAPI.Repositories.DataModels;
using ReportManagementAPI.Repositories.Repositories.Interface;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private IQueueService _queueService;
    private IMapper _mapper;
    public ReportService(IReportRepository reportRepository, IMapper mapper, IQueueService queueService)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
        _queueService = queueService;
    }

    public async Task<List<ReportDto>> GetList()
    {
        var domainReports = await _reportRepository.GetAllAsync();
        return _mapper.Map<List<ReportDto>>(domainReports);
    }
    

    public async Task UpdateReport(ReportDto report)
    {
        var domainReport = await _reportRepository.GetByIdAsync(report.UUID);
        domainReport.ReportState = report.ReportState;
        domainReport.Path = report.Path;

        await _reportRepository.Update(domainReport);
    }

    public async Task<ReportDto> RequestReport()
    {
        var report = await _reportRepository.Insert(new Report()
        {
            ReportState = ReportState.Requested,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        });
        _queueService.PushQueue(report.UUID.ToString());
        
        return _mapper.Map<ReportDto>(report);
    }
}