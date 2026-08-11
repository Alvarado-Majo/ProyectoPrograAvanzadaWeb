using CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Reviews
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ProyectoDBContext _context;

        public ReviewRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Reviews>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Entities.Reviews?> GetReviewById(int id)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task<List<Entities.Reviews>> GetReviewsByMovieId(int movieId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<Entities.Reviews>> GetReviewsByUserId(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CreateReview(Entities.Reviews review)
        {
            if (review == null)
                return false;

            await _context.Reviews.AddAsync(review);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateReview(Entities.Reviews review)
        {
            if (review == null)
                return false;

            var existingReview =
                await _context.Reviews.FindAsync(review.ReviewId);

            if (existingReview == null)
                return false;

            existingReview.IsLike = review.IsLike;
            existingReview.Comment = review.Comment;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReview(int id)
        {
            var entity = await _context.Reviews.FindAsync(id);

            if (entity == null)
                return false;

            _context.Reviews.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}