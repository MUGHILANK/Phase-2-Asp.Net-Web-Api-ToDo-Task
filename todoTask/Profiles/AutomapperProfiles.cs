using AutoMapper;
using todoTask.Models.DTO;
using todoTask.Models.Domain;

namespace todoTask.Profiles
{
    public class AutomapperProfiles : Profile
    {
        //Create ctor
        public AutomapperProfiles()
        {
            //Create mapping to DTO to Domain
            CreateMap<AddTodotaskDto,Todotask>().ReverseMap();
            CreateMap<UpdateTodotaskDto, Todotask>().ReverseMap();
        }
    }
}
