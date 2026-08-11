using System;

namespace CineStreamCR.BLL.DTO.Review
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public bool IsLike { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }

    // Resumen de reviews de una película: total, likes/dislikes y el
    public class ReviewSummaryDTO
    {
        public int MovieId { get; set; }
        public int TotalReviews { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public decimal? MovieRating { get; set; }
    }
}