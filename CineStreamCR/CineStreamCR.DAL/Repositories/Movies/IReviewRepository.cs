using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Reviews
{
    public interface IReviewRepository
    {
        Task<List<Entities.Reviews>> GetReviews();

        Task<Entities.Reviews?> GetReviewById(int id);

        Task<bool> CreateReview(Entities.Reviews review);

        Task<bool> UpdateReview(Entities.Reviews review);

        Task<bool> DeleteReview(int id);

        // Consultas

        Task<List<Entities.Reviews>> GetReviewsByMovieId(int movieId);

        Task<List<Entities.Reviews>> GetReviewsByUserId(int userId);
    }
}