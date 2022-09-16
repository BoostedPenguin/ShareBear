using AutoMapper;
using ShareBear.Data.Models;
using ShareBear.Dtos;

namespace ShareBear.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContainerHubsDto, ContainerHubs>();
        }
    }
}
