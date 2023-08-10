using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    
    // GET
    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public IActionResult Index()
    {
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> SaveHotel(HotelSaveRequest hotel)
    {
        var addedHotel = await _hotelService.AddHotelAsync(hotel);
        return Ok(addedHotel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotel(string id)
    {
        var hotel = await _hotelService.GetHotelByIDAsync(id); 
        return Ok(hotel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveHotel(string id)
    {
        await _hotelService.RemoveHotelAsync(id);
        return NoContent();
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetHotelList(int pageNumber, int pageSize)
    {
        var hotels = await _hotelService.GetHotelList(pageNumber, pageSize);
        return Ok(hotels);
    }
}