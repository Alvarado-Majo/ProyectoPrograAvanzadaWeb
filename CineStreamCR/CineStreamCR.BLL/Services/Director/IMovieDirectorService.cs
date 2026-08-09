using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.BLL.DTO.Movie;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Director
{
    public interface IMovieDirectorService
    {
        Task<Answer<MovieDirectorSummaryDTO?>> AssignDirectorToMovie(DTO.Director.AssignDirectorToMovieDTO dto);
        Task<Answer<bool>> RemoveDirectorFromMovie(int movieId, int directorId);

        // Consultas
        Task<Answer<List<MovieDirectorSummaryDTO>>> GetDirectorsByMovieId(int movieId);
        Task<Answer<List<MovieDirectorSummaryDTO>>> GetMoviesByDirectorId(int directorId);

    }
}
