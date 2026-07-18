using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Categories
{
    public class MovieCategoryRepository : IMovieCategoryRepository
    {
        private readonly ProyectoDBContext _context;

        public MovieCategoryRepository(
            ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<MovieCategories?> GetByMovieAndCategory(int movieId, int categoryId)
        {
            return await _context.MovieCategories
                .FirstOrDefaultAsync(mc =>
                    mc.MovieId == movieId &&
                    mc.CategoryId == categoryId);
        }

        public async Task<bool> AssignCategoryToMovie(MovieCategories movieCategory)
        {
            if (movieCategory == null)
                return false;

            var exists =
                await GetByMovieAndCategory(
                    movieCategory.MovieId,
                    movieCategory.CategoryId);

            if (exists != null)
                return false;

            await _context.MovieCategories
                .AddAsync(movieCategory);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveCategoryFromMovie(int movieId, int categoryId)
        {
            var entity =
                await GetByMovieAndCategory(
                    movieId,
                    categoryId);

            if (entity == null)
                return false;

            _context.MovieCategories.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MovieCategories>> GetByMovieId(int movieId)
        {
            return await _context.MovieCategories
                .Include(mc => mc.Category)
                .Where(mc => mc.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<MovieCategories>> GetByCategoryId(int categoryId)
        {
            return await _context.MovieCategories
                .Include(mc => mc.Movie)
                .Where(mc => mc.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}