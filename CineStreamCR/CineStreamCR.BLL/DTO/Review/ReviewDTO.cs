using System;

namespace CineStreamCR.BLL.DTO.Review
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }
}