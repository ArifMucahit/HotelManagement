using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;

namespace HotelManagementAPI.Repositories.Repository;

public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
{
    private HotelManagementContext _context;
    public HotelRepository(HotelManagementContext context) : base(context)
    {
        _context = context;
    }


    public async Task<List<ReportDto>> GetHotelReport()
    {
        var query = from h in _context.Hotels
            join c in _context.ContactInfos on h.UUID equals c.HotelId into contactGroup
            from contact in contactGroup.DefaultIfEmpty()
            group new { h, contact } by h.City into grouped
            select new ReportDto()
            {
                City = grouped.Key,
                HotelCount = grouped.Select(x => x.h.UUID).Distinct().Count(),
                ContactCount = grouped.Select(x => x.contact.UUID).Count()
            };

        return query.ToList();
    }
}