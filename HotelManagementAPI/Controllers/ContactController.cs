using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;


    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> AddContactInfo(ContactSaveRequest contact)
    {
        var contactInfo = _contactService.AddContactInfoAsync(contact, HttpContext.RequestAborted);
        return Ok(contactInfo);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveContactInfo(string id)
    {
        await _contactService.RemoveContactInfoAsync(id);
        return NoContent();
    }
}