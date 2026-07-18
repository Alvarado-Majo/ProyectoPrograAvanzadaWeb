using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.WatchLists
{
    public interface IWatchListMoviesRepository
    {
        Task<WatchListMovies?> GetByWatchListAndMovie(int watchListId,int movieId);
        Task<bool> AddMovieToWatchList(WatchListMovies watchListMovie);
        Task<bool> RemoveMovieFromWatchList(int watchListId,int movieId);

        // Consultas
        Task<List<WatchListMovies>> GetByWatchListId(int watchListId);
        Task<List<WatchListMovies>> GetByMovieId(int movieId);
    }
}