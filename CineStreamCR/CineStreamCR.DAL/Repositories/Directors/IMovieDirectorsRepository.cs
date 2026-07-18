using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Directors
{
    public interface IMovieDirectorsRepository
    {
        Task<MovieDirectors?> GetByMovieAndDirector( int movieId, int directorId);
        Task<bool> AssignDirectorToMovie( MovieDirectors movieDirector);
        Task<bool> RemoveDirectorFromMovie( int movieId, int directorId);

        // Consultas
        Task<List<MovieDirectors>> GetByMovieId(int movieId);
        Task<List<MovieDirectors>> GetByDirectorId( int directorId);
    }
}