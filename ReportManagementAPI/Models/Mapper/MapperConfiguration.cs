using AutoMapper;
using ReportManagementAPI.Models.Dto;
using ReportManagementAPI.Repositories.DataModels;

namespace ReportManagementAPI.Models.Mapper;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<ReportDto, Report>().ReverseMap();
    }
}