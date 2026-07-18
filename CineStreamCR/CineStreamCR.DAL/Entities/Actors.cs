
using System.ComponentModel.DataAnnotations.Schema;


namespace CineStreamCR.DAL.Entities
{
    [Table("Actors")]
    public partial class Actors
    {
       public int ActorId { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string Nationality { get; set; } = string.Empty;
       public string Biography { get; set; } = string.Empty;
       public DateOnly BirthDate { get; set; } 
       public string PictureImg { get; set; } = string.Empty;
       public byte IsActive { get; set; } = 1;
       public virtual ICollection<MovieActors> MovieActors { get; set; } = new List<MovieActors>();
    }


}
