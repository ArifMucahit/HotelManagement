using AutoMapper;
using HotelManagementAPI.DataModels;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Repositories.Repository.Interface;
using HotelManagementAPI.Services.Interface;

namespace HotelManagementAPI.Services;

public class ManagerService : IManagerService
{
    private IHotelManagerRepository _managerRepository;
    private ICacheManager _cache;
    private IMapper _mapper;
    private string _cacheTemplate = "{id}-manager";

    public ManagerService(IHotelManagerRepository managerRepository, ICacheManager cache, IMapper mapper, string cacheTemplate)
    {
        _managerRepository = managerRepository;
        _cache = cache;
        _mapper = mapper;
        _cacheTemplate = cacheTemplate;
    }

    public async Task<ManagerDto> AddManagerAsync(ManagerSaveRequest manager, CancellationToken ct = default)
    {
        var domainManager = _mapper.Map<HotelManagers>(manager);

        var inserted = _mapper.Map<ManagerDto>( await _managerRepository.Insert(domainManager));
        _cache.AddAsync(string.Format(_cacheTemplate, inserted.UUID), inserted);
        return inserted;
    }

    public async Task<List<ManagerDto>> ListManagersAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var list = await _managerRepository.GetAllAsync(pageNumber, pageSize, ct);
        return _mapper.Map<List<ManagerDto>>(list);
    }
}