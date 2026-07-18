using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Director;

namespace CineStreamCR.BLL
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {

            // Conversiones de tipo para DateOnly <-> DateTime.
            // AutoMapper no sabe convertir entre estos dos tipos por sí solo,
            // así que hay que decírselo explícitamente. Esto se aplica
            // automáticamente a cualquier CreateMap<> de abajo que tenga
            // una propiedad DateOnly de un lado y DateTime del otro
            // (incluye las versiones nullable DateOnly? y DateTime?).

            CreateMap<DateOnly, DateTime>()
                .ConvertUsing(d => d.ToDateTime(TimeOnly.MinValue));

            CreateMap<DateTime, DateOnly>()
                .ConvertUsing(dt => DateOnly.FromDateTime(dt));

            CreateMap<DateOnly?, DateTime?>()
                .ConvertUsing(d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);

            CreateMap<DateTime?, DateOnly?>()
                .ConvertUsing(dt => dt.HasValue ? DateOnly.FromDateTime(dt.Value) : (DateOnly?)null);


            //Actor DTO Mapping
            CreateMap<DAL.Entities.Actors, ActorDTO>().ReverseMap();
            CreateMap<DAL.Entities.Actors, CreateActorDTO>().ReverseMap();


            //Director DTO Mapping
            CreateMap<DAL.Entities.Directors, DirectorDTO>().ReverseMap();
            CreateMap<DAL.Entities.Directors, CreateDirectorDTO>().ReverseMap();


        }
    }
}