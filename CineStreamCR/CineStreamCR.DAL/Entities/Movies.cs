using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities;

[Table("Movies")]
public partial class Movies
{
    public int MovieId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal? MovieRating { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int DurationMinutes { get; set; }
    public string PosterImg { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte IsActive { get; set; } = 1;
    public virtual ICollection<MovieActors> MovieActors { get; set; } = new List<MovieActors>();
    public virtual ICollection<MovieDirectors> MovieDirectors { get; set; } = new List<MovieDirectors>();
    public virtual ICollection<MovieCategories> MovieCategories { get; set; } = new List<MovieCategories>();
    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
    public virtual ICollection<WatchListMovies> WatchListMovies { get; set; } = new List<WatchListMovies>();
}