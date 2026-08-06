using System;
using System.Collections.Generic;

namespace CineStreamCR.BLL.DTO.Movie
{
    public enum MovieSortField
    {
        Title,
        ReleaseYear,
        MovieRating
    }

    // Parámetros de entrada para la vista de Catálogo: buscador en tiempo real, filtro por género y/o año y ordenamiento.
    public class MovieFilterDTO
    {
        public string? SearchTitle { get; set; }
        public int? CategoryId { get; set; }
        public int? ReleaseYear { get; set; }
        public MovieSortField SortBy { get; set; } = MovieSortField.Title;
        public bool SortDescending { get; set; } = false;

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        private int _pageSize = 12;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 12 : value;
        }
    }

    // Resultado paginado.
    public class MovieCatalogResultDTO
    {
        public List<MovieDTO> Movies { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}