using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.DAL.Entities;
using CineStreamCR.DAL.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Movie
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<Answer<List<MovieDTO>>> GetAllMovies()
        {
            var movies = await _movieRepository.GetMovies();
            var answer = new Answer<List<MovieDTO>>();
            answer.Dato = _mapper.Map<List<MovieDTO>>(movies);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<MovieDTO>> GetCreateMovie(CreateMovieDTO movie)
        {
            if (movie == null)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Movie inválido.",
                    codigo = 400
                };
            }

            var existing = await _movieRepository.GetMovieByTitle(movie.Title);
            if (existing != null)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = false,
                    mensaje = "La Movie ya existe.",
                    codigo = 400
                };
            }

            var newMovie = _mapper.Map<Movies>(movie);
            newMovie.IsActive = 1;

            bool result = await _movieRepository.CreateMovie(newMovie);
            if (result)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Movie creada exitosamente.",
                    Dato = _mapper.Map<MovieDTO>(newMovie),
                    codigo = 201
                };
            }
            return new Answer<MovieDTO>
            {
                EsCorrecto = false,
                mensaje = "Error al crear la movie.",
                codigo = 500
            };
        }

        public async Task<Answer<bool>> GetDeleteMovie(int id)
        {
            var answer = new Answer<bool>();
            bool result = await _movieRepository.DeleteMovie(id);
            if (result)
            {
                answer.EsCorrecto = true;
                answer.mensaje = "Movie eliminada exitosamente.";
                answer.codigo = 200;
                return answer;
            }
            answer.EsCorrecto = false;
            answer.mensaje = "Error al eliminar la movie.";
            answer.codigo = 500;
            return answer;
        }

        public async Task<Answer<MovieDTO?>> GetMovieById(int id)
        {
            var answer = new Answer<MovieDTO?>();
            var movieEntity = await _movieRepository.GetMovieById(id);
            if (movieEntity == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Movie no encontrada.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<MovieDTO?>(movieEntity);
            return answer;
        }

        public async Task<Answer<MovieDTO?>> GetMovieByTitle(string title)
        {
            var answer = new Answer<MovieDTO?>();
            var movieEntity = await _movieRepository.GetMovieByTitle(title);
            if (movieEntity == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Movie no encontrada.";
                answer.codigo = 404;
                return answer;

            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<MovieDTO?>(movieEntity);
            return answer;
        }

        public async Task<Answer<MovieDetailDTO?>> GetMovieDetailsById(int id)
        {
            var answer = new Answer<MovieDetailDTO?>();

            var movieEntity =
                await _movieRepository.GetMovieDetailsById(id);

            if (movieEntity == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Movie not found.";
                answer.codigo = 404;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<MovieDetailDTO>(movieEntity);
            answer.codigo = 200;

            return answer;
        }

        public async Task<Answer<List<MovieDTO>>> GetMoviesByDirectorId(int directorId)
        {
            var answer = new Answer<List<MovieDTO?>>();
            var directors = await _movieRepository.GetMoviesByDirectorId(directorId);
            answer.Dato = _mapper.Map<List<MovieDTO?>>(directors);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<MovieDTO>>> GetMoviesByActorId(int actorId)
        {
            var answer = new Answer<List<MovieDTO?>>();
            var actors = await _movieRepository.GetMoviesByActorId(actorId);
            answer.Dato = _mapper.Map<List<MovieDTO?>>(actors);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<MovieDTO>> GetUpdateMovie(int id, CreateMovieDTO movie)
        {
            var answer = await _movieRepository.GetMovieById(id);
            if (answer == null)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Movie not found",
                    codigo = 404
                };
            }
            var movieTitle = await _movieRepository.GetMovieByTitle(movie.Title);
            if (movieTitle != null && movieTitle.MovieId != id)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Movie with the same name already exists",
                    codigo = 400
                };
            }
            answer.Title = movie.Title;
            answer.Synopsis = movie.Synopsis;
            answer.ReleaseYear = movie.ReleaseYear;
            answer.DurationMinutes = movie.DurationMinutes;
            answer.PosterImg = movie.PosterImg;
            answer.VideoUrl = movie.VideoUrl;
            answer.Nationality = movie.Nationality;


            answer.IsActive = movie.IsActive;

            bool result = await _movieRepository.UpdateMovie(answer);

            if (result)
            {
                return new Answer<MovieDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Movie updated successfully",
                    Dato = _mapper.Map<MovieDTO>(answer),
                    codigo = 200
                };
            }
            return new Answer<MovieDTO>
            {
                EsCorrecto = false,
                mensaje = "Error updating movie",
                codigo = 500
            };
        }
    }
}
