using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Directors
{
    public class MovieDirectorsRepository : IMovieDirectorsRepository
    {
        private readonly ProyectoDBContext _context;

        public MovieDirectorsRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<MovieDirectors?> GetByMovieAndDirector(int movieId,int directorId)
        {
            return await _context.MovieDirectors
                .FirstOrDefaultAsync(md =>
                    md.MovieId == movieId &&
                    md.DirectorId == directorId);
        }

        public async Task<bool> AssignDirectorToMovie(MovieDirectors movieDirector)
        {
            if (movieDirector == null)
                return false;

            var exists =
                await GetByMovieAndDirector(
                    movieDirector.MovieId,
                    movieDirector.DirectorId);

            if (exists != null)
                return false;

            await _context.MovieDirectors
                .AddAsync(movieDirector);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveDirectorFromMovie(int movieId, int directorId)
        {
            var entity =
                await GetByMovieAndDirector(
                    movieId,
                    directorId);

            if (entity == null)
                return false;

            _context.MovieDirectors.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MovieDirectors>> GetByMovieId(int movieId)
        {
            return await _context.MovieDirectors
                .Include(md => md.Director)
                .Where(md => md.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<MovieDirectors>> GetByDirectorId(int directorId)
        {
            return await _context.MovieDirectors
                .Include(md => md.Movie)
                .Where(md => md.DirectorId == directorId)
                .ToListAsync();
        }
    }
}