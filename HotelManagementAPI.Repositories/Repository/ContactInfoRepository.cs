using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;

namespace HotelManagementAPI.Repositories.Repository;

public class ContactInfoRepository : BaseRepository<ContactInfo>, IContactInfoRepository
{
    public ContactInfoRepository(HotelManagementContext context) : base(context)
    {
    }
}