using AutoMapper;
using ReportManagementAPI.Models.Dto;
using ReportManagementAPI.Repositories.DataModels;
using ReportManagementAPI.Repositories.Repositories.Interface;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private IMapper _mapper;
    public ReportService(IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<List<ReportDto>> GetList()
    {
        var domainReports = await _reportRepository.GetAllAsync();
        return _mapper.Map<List<ReportDto>>(domainReports);
    }

    public async Task UpdateReportState(string id, ReportState state)
    {
        var domainReport = await _reportRepository.GetByIdAsync(Guid.Parse(id));
        domainReport.ReportState = state;

        await _reportRepository.Update(domainReport);
    }

    public async Task UpdateReportPath(string id, string path)
    {
        var domainReport = await _reportRepository.GetByIdAsync(Guid.Parse(id));
        domainReport.ReportState = ReportState.Done;
        domainReport.Path = path;

        await _reportRepository.Update(domainReport);
    }
}