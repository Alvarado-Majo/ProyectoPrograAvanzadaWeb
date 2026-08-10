using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Movie;

namespace StreamingApp.Models
{
    public class ActorDetailViewModel
    {
        public ActorDTO Actor { get; set; } = new();

        public List<MovieCastMemberDTO> Movies { get; set; } = new();
    }
}
