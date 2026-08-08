using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Movie;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Movie
{
    public interface IMovieService
    {
        Task<Answer<List<MovieDTO>>> GetAllMovies();

        Task<Answer<MovieDTO?>> GetMovieById(int id);

        Task<Answer<MovieDTO?>> GetMovieByTitle(string title);

        Task<Answer<MovieDTO>> GetCreateMovie(CreateMovieDTO movie);

        Task<Answer<MovieDTO>> GetUpdateMovie(int id, CreateMovieDTO movie);

        Task<Answer<bool>> GetDeleteMovie(int id);

        // Consultas específicas del negocio para usar después.

        Task<Answer<MovieDTO?>> GetMovieDetallesById(int id);

        Task<Answer<List<MovieDTO>>> GetMoviesByDirectorId(int directorId);

        Task<Answer<List<MovieDTO>>> GetMoviesByActorId(int actorId);
    }
}
