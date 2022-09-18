using AutoMapper;
using ShareBear.Data.Models;
using ShareBear.Dtos;
using ShareBear.Services;

namespace ShareBear.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContainerHubs, ContainerHubsDto>();
            CreateMap<ContainerFiles, ContainerFilesDto>();
        }
    }
}
