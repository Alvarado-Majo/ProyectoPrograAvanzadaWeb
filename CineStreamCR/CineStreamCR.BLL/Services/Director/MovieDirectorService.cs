using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.DAL.Repositories.Directors;
using CineStreamCR.DAL.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Director
{
    public class MovieDirectorService : IMovieDirectorService
    {
        private readonly IMovieDirectorsRepository _movieDirectorsRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieDirectorService(
            IMovieDirectorsRepository movieDirectorsRepository,
            IDirectorRepository directorRepository,
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            _movieDirectorsRepository = movieDirectorsRepository;
            _directorRepository = directorRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Answer<MovieDirectorSummaryDTO?>> AssignDirectorToMovie(DTO.Director.AssignDirectorToMovieDTO dto)
        {
            var answer = new Answer<MovieDirectorSummaryDTO?>();

            var movie = await _movieRepository.GetMovieById(dto.MovieId);
            if (movie == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La película no existe.";
                answer.codigo = 404;
                return answer;
            }

            var director = await _directorRepository.GetDirectorById(dto.DirectorId);
            if (director == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El director no existe.";
                answer.codigo = 404;
                return answer;
            }

            var existing = await _movieDirectorsRepository.GetByMovieAndDirector(dto.MovieId, dto.DirectorId);
            if (existing != null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El director ya está asignado a esta película.";
                answer.codigo = 400;
                return answer;
            }

            var newMovieDirector = new DAL.Entities.MovieDirectors
            {
                MovieId = dto.MovieId,
                DirectorId = dto.DirectorId
            };

            bool result = await _movieDirectorsRepository.AssignDirectorToMovie(newMovieDirector);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al asignar el director a la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Director asignado correctamente.";
            answer.codigo = 201;
            answer.Dato = new MovieDirectorSummaryDTO
            {
                MovieId = dto.MovieId,
                DirectorId = dto.DirectorId,
                FullName = $"{director.FirstName} {director.LastName}",
                PictureImg = director.PictureImg,
                MovieTitle = movie.Title,
            };
            return answer;
        }

        public async Task<Answer<bool>> RemoveDirectorFromMovie(int movieId, int directorId)
        {
            var answer = new Answer<bool>();

            var existing = await _movieDirectorsRepository.GetByMovieAndDirector(movieId, directorId);
            if (existing == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "El director no está asignado a esta película.";
                answer.codigo = 404;
                return answer;
            }

            bool result = await _movieDirectorsRepository.RemoveDirectorFromMovie(movieId, directorId);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al quitar el director de la película.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Director removido correctamente.";
            answer.codigo = 200;
            answer.Dato = true;
            return answer;
        }

        public async Task<Answer<List<MovieDirectorSummaryDTO>>> GetDirectorsByMovieId(int movieId)
        {
            var answer = new Answer<List<MovieDirectorSummaryDTO>>();
            var movieDirectors = await _movieDirectorsRepository.GetByMovieId(movieId);
            answer.Dato = _mapper.Map<List<MovieDirectorSummaryDTO>>(movieDirectors);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<MovieDirectorSummaryDTO>>> GetMoviesByDirectorId(int directorId)
        {
            var answer = new Answer<List<MovieDirectorSummaryDTO>>();
            var movieDirectors = await _movieDirectorsRepository.GetByDirectorId(directorId);
            answer.Dato = _mapper.Map<List<MovieDirectorSummaryDTO>>(movieDirectors);
            answer.EsCorrecto = true;
            return answer;
        }
    }
}
