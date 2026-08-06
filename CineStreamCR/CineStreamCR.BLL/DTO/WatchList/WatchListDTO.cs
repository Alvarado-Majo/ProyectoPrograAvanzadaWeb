using System;
using System.Collections.Generic;

namespace CineStreamCR.BLL.DTO.WatchList
{
    // Para listar WatchLists de un usuario.
    public class WatchListDTO
    {
        public int WatchListId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int MovieCount { get; set; }
    }

    // Película dentro de un WatchList (por si usamos cards para presentarlos).
    public class WatchListMovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PosterImg { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int DurationMinutes { get; set; }
        public decimal? MovieRating { get; set; }
    }

    // Ver todas las películas de un WatchList.
    public class WatchListDetailDTO
    {
        public int WatchListId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<WatchListMovieDTO> Movies { get; set; } = new();
    }
}