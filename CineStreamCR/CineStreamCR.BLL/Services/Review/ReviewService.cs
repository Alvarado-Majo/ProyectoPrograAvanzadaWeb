using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Review;
using CineStreamCR.DAL.Repositories.Movies;
using CineStreamCR.DAL.Repositories.Reviews;

namespace CineStreamCR.BLL.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public ReviewService(
            IReviewRepository reviewRepository,
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Answer<List<ReviewDTO?>>> GetReviewsByMovieIdAsync(int movieId)
        {
            var answer = new Answer<List<ReviewDTO?>>();
            var reviews = await _reviewRepository.GetReviewsByMovieId(movieId);
            answer.Dato = MapReviews(reviews);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<ReviewDTO?>>> GetReviewsByUserIdAsync(int userId)
        {
            var answer = new Answer<List<ReviewDTO?>>();
            var reviews = await _reviewRepository.GetReviewsByUserId(userId);
            answer.Dato = MapReviews(reviews);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<ReviewDTO?>> GetReviewByIdAsync(int id)
        {
            var answer = new Answer<ReviewDTO?>();
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Review not found.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<ReviewDTO?>(review);
            return answer;
        }

        public async Task<Answer<ReviewDTO>> GetCreateReviewAsync(CreateReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Invalid review.",
                    codigo = 400
                };
            }

            var movie = await _movieRepository.GetMovieById(reviewDTO.MovieId);
            if (movie == null)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "The movie you are trying to review does not exist.",
                    codigo = 404
                };
            }

            // Un usuario sólo puede calificar una vez cada película
            var existingReviews = await _reviewRepository.GetReviewsByMovieId(reviewDTO.MovieId);
            if (existingReviews.Any(r => r.UserId == reviewDTO.UserId))
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "You have already reviewed this movie.",
                    codigo = 400
                };
            }

            var newReview = new DAL.Entities.Reviews
            {
                UserId = reviewDTO.UserId,
                MovieId = reviewDTO.MovieId,
                IsLike = reviewDTO.IsLike,
                Comment = reviewDTO.Comment ?? string.Empty,
                ReviewDate = DateTime.Now
            };

            bool result = await _reviewRepository.CreateReview(newReview);
            if (!result)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error creating the review.",
                    codigo = 500
                };
            }

            await RecalculateMovieRatingAsync(reviewDTO.MovieId);

            return new Answer<ReviewDTO>
            {
                EsCorrecto = true,
                mensaje = "Review created successfully.",
                Dato = _mapper.Map<ReviewDTO>(newReview),
                codigo = 201
            };
        }

        public async Task<Answer<ReviewDTO>> GetUpdateReviewAsync(int id, CreateReviewDTO reviewDTO)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Review not found.",
                    codigo = 404
                };
            }

            review.IsLike = reviewDTO.IsLike;
            review.Comment = reviewDTO.Comment ?? string.Empty;

            bool result = await _reviewRepository.UpdateReview(review);
            if (!result)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error updating the review.",
                    codigo = 500
                };
            }

            await RecalculateMovieRatingAsync(review.MovieId);

            return new Answer<ReviewDTO>
            {
                EsCorrecto = true,
                mensaje = "Review updated successfully.",
                Dato = _mapper.Map<ReviewDTO>(review),
                codigo = 200
            };
        }

        public async Task<Answer<ReviewDTO>> GetDeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Review not found.",
                    codigo = 404
                };
            }

            int movieId = review.MovieId;

            bool result = await _reviewRepository.DeleteReview(id);
            if (!result)
            {
                return new Answer<ReviewDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error deleting the review.",
                    codigo = 500
                };
            }

            await RecalculateMovieRatingAsync(movieId);

            return new Answer<ReviewDTO>
            {
                EsCorrecto = true,
                mensaje = "Review deleted successfully.",
                codigo = 200
            };
        }

        public async Task<Answer<ReviewSummaryDTO>> GetReviewSummaryByMovieIdAsync(int movieId)
        {
            var answer = new Answer<ReviewSummaryDTO>();
            var reviews = await _reviewRepository.GetReviewsByMovieId(movieId);
            var movie = await _movieRepository.GetMovieById(movieId);

            int likes = reviews.Count(r => r.IsLike);

            answer.EsCorrecto = true;
            answer.Dato = new ReviewSummaryDTO
            {
                MovieId = movieId,
                TotalReviews = reviews.Count,
                Likes = likes,
                Dislikes = reviews.Count - likes,
                MovieRating = movie?.MovieRating
            };
            return answer;
        }

        
        private async Task RecalculateMovieRatingAsync(int movieId)
        {
            var movie = await _movieRepository.GetMovieById(movieId);
            if (movie == null)
                return;

            var reviews = await _reviewRepository.GetReviewsByMovieId(movieId);

            if (reviews.Count == 0)
            {
                movie.MovieRating = null;
            }
            else
            {
                int likes = reviews.Count(r => r.IsLike);
                movie.MovieRating = Math.Round((decimal)likes / reviews.Count * 10, 1);
            }

            await _movieRepository.UpdateMovie(movie);
        }

        private List<ReviewDTO?> MapReviews(List<DAL.Entities.Reviews> reviews)
        {
           
            return reviews.Select(r =>
            {
                var dto = _mapper.Map<ReviewDTO>(r);
                if (string.IsNullOrWhiteSpace(dto.UserFullName) && r.User != null)
                    dto.UserFullName = $"{r.User.FirstName} {r.User.LastName}";
                return (ReviewDTO?)dto;
            }).ToList();
        }
    }
}