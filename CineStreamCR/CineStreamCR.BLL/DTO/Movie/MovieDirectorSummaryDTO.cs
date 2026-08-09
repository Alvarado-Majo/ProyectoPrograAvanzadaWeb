namespace CineStreamCR.BLL.DTO.Movie
{
    // Resumen del director para mostrar en eldetalle de película, enlaza al perfil del director sin toda la bio del director.
    public class MovieDirectorSummaryDTO
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PictureImg { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
    }

    // Miembro del elenco más personaje interpretado en la película seleccionada.
    public class MovieCastMemberDTO
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string ActorFullName { get; set; } = string.Empty;
        public string ActorPictureImg { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string CharacterName { get; set; } = string.Empty;
    }

    // Categoría y/o género asignado a la movie.
    public class MovieCategorySummaryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class AssignDirectorToMovieDTO
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
    }
    public class AssignActorToMovieDTO
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
    }
}