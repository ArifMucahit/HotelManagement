using AutoMapper;
using HotelManagementAPI.DataModels;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Repositories.Repository.Interface;
using HotelManagementAPI.Services.Interface;

namespace HotelManagementAPI.Services;

public class HotelService : IHotelService
{
    private IHotelRepository _hotelRepository;
    private IContactInfoRepository _contactInfoRepository;
    private ICacheManager _cache;
    private IMapper _mapper;
    private string _cacheTemplate = "{0}-hotel";

    public HotelService(IHotelRepository hotelRepository, ICacheManager cache, IMapper mapper, IContactInfoRepository contactInfoRepository)
    {
        _hotelRepository = hotelRepository;
        _cache = cache;
        _mapper = mapper;
        _contactInfoRepository = contactInfoRepository;
    }

    public async Task<HotelDTO> AddHotelAsync(HotelSaveRequest hotel, CancellationToken ct = default)
    {
        var domainHotel = _mapper.Map<Hotel>(hotel);

        var inserted = _mapper.Map<HotelDTO>(await _hotelRepository.Insert(domainHotel, ct));

        _cache.AddAsync(string.Format(_cacheTemplate,inserted.UUID.ToString()), inserted );
        return inserted;
    }

    public async Task<HotelDetails> GetHotelByIDAsync(string id, CancellationToken ct = default)
    { //TODO include contact info!!!!
        var key = string.Format(_cacheTemplate, id);
        var hotel = await _cache.GetOrAdd(key, async () =>
        {
            var hotel =  await _hotelRepository.GetByIdAsync(Guid.Parse(id), ct);
            var contact = await _contactInfoRepository.GetByFilter(x => x.HotelId == hotel.UUID);
            var result = _mapper.Map<HotelDetails>(hotel);
            result.ContactInfos = _mapper.Map<List<HotelContactInfo>>(contact);
            return result;
        });
        return hotel;
    }

    public async Task RemoveHotelAsync(string id, CancellationToken ct = default)
    {
        var key = string.Format(_cacheTemplate, id);
        var hotel = await _hotelRepository.GetByIdAsync(Guid.Parse(id), ct);

        await _hotelRepository.Delete(hotel, ct);
        _cache.Remove(key);
    }

    public async Task<List<HotelDTO>> GetHotelList(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var hotelList = await _hotelRepository.GetAllAsync(pageNumber, pageSize, ct);

        return _mapper.Map<List<HotelDTO>>(hotelList);
    }
}