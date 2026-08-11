using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Review;

namespace CineStreamCR.BLL.Services.Review
{
    public interface IReviewService
    {
        Task<Answer<List<ReviewDTO?>>> GetReviewsByMovieIdAsync(int movieId);
        Task<Answer<List<ReviewDTO?>>> GetReviewsByUserIdAsync(int userId);
        Task<Answer<ReviewDTO?>> GetReviewByIdAsync(int id);
        Task<Answer<ReviewDTO>> GetCreateReviewAsync(CreateReviewDTO reviewDTO);
        Task<Answer<ReviewDTO>> GetUpdateReviewAsync(int id, CreateReviewDTO reviewDTO);
        Task<Answer<ReviewDTO>> GetDeleteReviewAsync(int id);
        Task<Answer<ReviewSummaryDTO>> GetReviewSummaryByMovieIdAsync(int movieId);
    }
}