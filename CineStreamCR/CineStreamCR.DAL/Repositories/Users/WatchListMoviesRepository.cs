using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.WatchLists
{
    public class WatchListMoviesRepository : IWatchListMoviesRepository
    {
        private readonly ProyectoDBContext _context;

        public WatchListMoviesRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<WatchListMovies?> GetByWatchListAndMovie(int watchListId, int movieId)
        {
            return await _context.WatchListMovies
                .FirstOrDefaultAsync(wm =>
                    wm.WatchListId == watchListId &&
                    wm.MovieId == movieId);
        }

        public async Task<bool> AddMovieToWatchList(WatchListMovies watchListMovie)
        {
            if (watchListMovie == null)
                return false;

            var exists =
                await GetByWatchListAndMovie(
                    watchListMovie.WatchListId,
                    watchListMovie.MovieId);

            if (exists != null)
                return false;

            await _context.WatchListMovies
                .AddAsync(watchListMovie);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveMovieFromWatchList(int watchListId, int movieId)
        {
            var entity =
                await GetByWatchListAndMovie(
                    watchListId,
                    movieId);

            if (entity == null)
                return false;

            _context.WatchListMovies.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<WatchListMovies>> GetByWatchListId(int watchListId)
        {
            return await _context.WatchListMovies
                .Include(wm => wm.Movie)
                .Where(wm => wm.WatchListId == watchListId)
                .ToListAsync();
        }

        public async Task<List<WatchListMovies>> GetByMovieId(int movieId)
        {
            return await _context.WatchListMovies
                .Include(wm => wm.WatchList)
                .Where(wm => wm.MovieId == movieId)
                .ToListAsync();
        }
    }
}