using AutoMapper;
using HotelManagementAPI.DataModels;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Repositories.Repository.Interface;
using HotelManagementAPI.Services.Interface;

namespace HotelManagementAPI.Services;

public class ContactService : IContactService
{
    private IContactInfoRepository _contactRepository;
    private ICacheManager _cache;
    private IMapper _mapper;
    private string _cacheTemplate = "{0}-contact";

    public ContactService(IContactInfoRepository contactRepository, ICacheManager cache, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<ContactDto> AddContactInfoAsync(ContactSaveRequest contact, CancellationToken ct = default)
    {
        var domainContact = _mapper.Map<ContactInfo>(contact);

        var inserted = _mapper.Map<ContactDto>(await _contactRepository.Insert(domainContact));

        _cache.AddAsync(string.Format(_cacheTemplate, inserted.UUID.ToString()), inserted);
        return inserted;
    }

    public async Task RemoveContactInfoAsync(string id)
    {
        var key = string.Format(_cacheTemplate, id);
        var domainContactInfo = await _contactRepository.GetByIdAsync(Guid.Parse(id));
        await _contactRepository.Delete(domainContactInfo);
        _cache.Remove(key);
    }

    public async Task<List<ContactDto>> GetContactList(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var contacts = await _contactRepository.GetAllAsync(pageNumber, pageSize, ct);
        return _mapper.Map<List<ContactDto>>(contacts);
    }
}