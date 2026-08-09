using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.DTO.WatchList;

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

            //Movie DTO Mapping
            CreateMap<DAL.Entities.Movies, MovieDTO>().ReverseMap();
            CreateMap<DAL.Entities.Movies, CreateMovieDTO>().ReverseMap();


            //WatchList DTO Mapping
            CreateMap<DAL.Entities.WatchLists, WatchListDTO>()
                .ForMember(dest => dest.MovieCount,
                    opt => opt.MapFrom(src => src.WatchListMovies.Count))
                .ReverseMap()
                .ForMember(dest => dest.WatchListMovies, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<DAL.Entities.WatchLists, CreateWatchListDTO>().ReverseMap();

            //WatchListMovies DTO Mapping (tabla compuesta)
            CreateMap<DAL.Entities.WatchListMovies, WatchListMovieDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.PosterImg, opt => opt.MapFrom(src => src.Movie.PosterImg))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.Movie.ReleaseYear))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Movie.DurationMinutes))
                .ForMember(dest => dest.MovieRating, opt => opt.MapFrom(src => src.Movie.MovieRating))
                .ReverseMap()
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.WatchList, opt => opt.Ignore());
        }

    }
    }
}