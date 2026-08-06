using System;

namespace CineStreamCR.BLL.DTO.Movie
{
    // DTO para el catálogo tipo cards y listados generales.
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal? MovieRating { get; set; }
        public int ReleaseYear { get; set; }
        public int DurationMinutes { get; set; }
        public string PosterImg { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public byte IsActive { get; set; } = 1;
    }
}