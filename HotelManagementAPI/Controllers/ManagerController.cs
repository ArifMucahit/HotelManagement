using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _managerService;


    public ManagerController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveManagerAsync(ManagerSaveRequest saveRequest)
    {
        var manager = await _managerService.AddManagerAsync(saveRequest);
        return Ok(manager);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListManagersAsync(int pageNumber, int pageSize)
    {
        return Ok();
    }
    
}