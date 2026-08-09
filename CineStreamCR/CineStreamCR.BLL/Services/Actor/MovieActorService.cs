using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.Services.Actor;
using CineStreamCR.DAL.Repositories.Actors;
using CineStreamCR.DAL.Repositories.Movies;

namespace CineStreamCR.BLL.Services.Actor
{
    public class MovieActorService : IMovieActorService
    {
        private readonly IMovieActorsRepository _movieActorsRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieActorService(
            IMovieActorsRepository movieActorsRepository,
            IActorRepository actorRepository,
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            _movieActorsRepository = movieActorsRepository;
            _actorRepository = actorRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Answer<MovieCastMemberDTO?>> AssignActorToMovie(AssignActorToMovieDTO dto)
        {
            var answer = new Answer<MovieCastMemberDTO?>();

            var movie = await _movieRepository.GetMovieById(dto.MovieId);
            if (movie == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La película no existe.";
                answer.codigo = 404;
                return answer;
            }

            var actor = await _actorRepository.GetActorById(dto.ActorId);
            if (actor == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El actor no existe.";
                answer.codigo = 404;
                return answer;
            }

            var existing = await _movieActorsRepository.GetByMovieAndActor(dto.MovieId, dto.ActorId);
            if (existing != null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El actor ya está asignado a esta película.";
                answer.codigo = 400;
                return answer;
            }

            var newMovieActor = new DAL.Entities.MovieActors
            {
                MovieId = dto.MovieId,
                ActorId = dto.ActorId,
                CharacterName = dto.CharacterName
            };

            bool result = await _movieActorsRepository.AssignActorToMovie(newMovieActor);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al asignar el actor a la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Actor asignado correctamente.";
            answer.codigo = 201;
            answer.Dato = new MovieCastMemberDTO
            {
                MovieId = dto.MovieId,
                ActorId = dto.ActorId,
                CharacterName = dto.CharacterName,
                ActorFullName = $"{actor.FirstName} {actor.LastName}",
                ActorPictureImg = actor.PictureImg,
                MovieTitle = movie.Title,
                
            };
            return answer;
        }

        public async Task<Answer<bool>> RemoveActorFromMovie(int movieId, int actorId)
        {
            var answer = new Answer<bool>();

            var existing = await _movieActorsRepository.GetByMovieAndActor(movieId, actorId);
            if (existing == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El actor no está asignado a esta película.";
                answer.codigo = 404;
                return answer;
            }

            bool result = await _movieActorsRepository.RemoveActorFromMovie(movieId, actorId);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al quitar el actor de la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Actor removido correctamente.";
            answer.codigo = 200;
            answer.Dato = true;
            return answer;
        }

        public async Task<Answer<MovieCastMemberDTO?>> UpdateCharacterName(int movieId, int actorId, string characterName)
        {
            var answer = new Answer<MovieCastMemberDTO?>();

            var existing = await _movieActorsRepository.GetByMovieAndActor(movieId, actorId);
            if (existing == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El actor no está asignado a esta película.";
                answer.codigo = 404;
                return answer;
            }

            bool result = await _movieActorsRepository.UpdateCharacterName(movieId, actorId, characterName);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al actualizar el nombre del personaje.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Personaje actualizado correctamente.";
            answer.codigo = 200;
            answer.Dato = _mapper.Map<MovieCastMemberDTO?>(existing);
            if (answer.Dato != null)
                answer.Dato.CharacterName = characterName;
            return answer;
        }

        public async Task<Answer<List<MovieCastMemberDTO>>> GetActorsByMovieId(int movieId)
        {
            var answer = new Answer<List<MovieCastMemberDTO>>();
            var movieActors = await _movieActorsRepository.GetByMovieId(movieId);
            answer.Dato = _mapper.Map<List<MovieCastMemberDTO>>(movieActors);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<MovieCastMemberDTO>>> GetMoviesByActorId(int actorId)
        {
            var answer = new Answer<List<MovieCastMemberDTO>>();
            var movieActors = await _movieActorsRepository.GetByActorId(actorId);
            answer.Dato = _mapper.Map<List<MovieCastMemberDTO>>(movieActors);
            answer.EsCorrecto = true;
            return answer;
        }
    }
}
