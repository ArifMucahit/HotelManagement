using AutoMapper;
using HotelManagementAPI.DataModels;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Models.Mapper;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<HotelDTO, Hotel>().ReverseMap();
        CreateMap<HotelSaveRequest, Hotel>().ReverseMap();
        
        CreateMap<ContactDto, ContactInfo>().ReverseMap();
        CreateMap<ContactSaveRequest, ContactInfo>().ReverseMap();

        CreateMap<ManagerDto, HotelManagers>().ReverseMap();
        CreateMap<ManagerSaveRequest, HotelManagers>().ReverseMap();
    }
}