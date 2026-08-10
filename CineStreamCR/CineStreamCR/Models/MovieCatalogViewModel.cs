using CineStreamCR.BLL.DTO.Category;
using CineStreamCR.BLL.DTO.Movie;

namespace StreamingApp.Models
{
    public class MovieCatalogViewModel
    {
        public List<MovieDTO> Movies { get; set; } = new();

        public List<CategoryDTO> Categories { get; set; } = new();

        public int? SelectedCategoryId { get; set; }

        public string Search { get; set; } = string.Empty;
    }
}
