using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportManagementAPI.Models.Dto;
using ReportManagementAPI.Services.Interface;

namespace ReportManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IQueueService _queueService;
    private readonly IReportService _reportService;

    public ReportController(IQueueService queueService, IReportService reportService)
    {
        _queueService = queueService;
        _reportService = reportService;
    }


    [HttpPost]
    public async Task<IActionResult> RequestReport()
    {
        var report = await _reportService.RequestReport();
        return Ok(report);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetReportList()
    {
        var reports = await _reportService.GetList();
        
        return Ok(reports);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReport(ReportDto dto)
    {
        var report = _reportService.UpdateReport(dto);
        return Ok(report);
    }
}