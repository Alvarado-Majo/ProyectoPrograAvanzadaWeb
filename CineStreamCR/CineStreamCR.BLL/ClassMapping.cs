using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Director;

namespace CineStreamCR.BLL
{
    public class ClassMapping : Profile
    {
        public ClassMapping() { 
        //Actor DTO Mapping
            CreateMap<DAL.Entities.Actors, ActorDTO>().ReverseMap();
            CreateMap<DAL.Entities.Actors, CreateActorDTO>().ReverseMap();

            
            //Director DTO Mapping
            CreateMap<DAL.Entities.Directors, DirectorDTO>().ReverseMap();
            CreateMap<DAL.Entities.Directors, CreateDirectorDTO>().ReverseMap();

           


        }
    }
}
