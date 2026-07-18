using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Movies
{
    public interface IMovieRepository
    {
        // CRUD

        Task<List<Entities.Movies>> GetMovies();

        Task<Entities.Movies?> GetMovieById(int id);

        Task<bool> CreateMovie(Entities.Movies movie);

        Task<bool> UpdateMovie(Entities.Movies movie);

        Task<bool> DeleteMovie(int id);

        // Consultas específicas del negocio para usar después.

        Task<Entities.Movies?> GetMovieDetallesById(int id);

        Task<List<Entities.Movies>> GetMoviesByDirectorId(int directorId);

        Task<List<Entities.Movies>> GetMoviesByActorId(int actorId);
    }
}