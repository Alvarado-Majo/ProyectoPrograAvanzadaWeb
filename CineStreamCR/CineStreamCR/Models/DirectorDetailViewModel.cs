using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.BLL.DTO.Movie;

namespace StreamingApp.Models
{
    public class DirectorDetailViewModel
    {
        public DirectorDTO Director { get; set; } = new();

        public List<MovieDTO> Movies { get; set; } = new();
    }
}