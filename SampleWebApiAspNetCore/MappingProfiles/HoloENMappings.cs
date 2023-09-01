using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class HoloENMappings : Profile
    {
        public HoloENMappings()
        {
            CreateMap<HoloENEntity, HoloENDto>().ReverseMap();
            CreateMap<HoloENEntity, HoloENUpdateDto>().ReverseMap();
            CreateMap<HoloENEntity, HoloENCreateDto>().ReverseMap();
        }
    }
}
