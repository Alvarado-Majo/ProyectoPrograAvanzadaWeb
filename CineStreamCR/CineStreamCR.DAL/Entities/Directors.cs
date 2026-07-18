using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineStreamCR.DAL.Entities
{
    [Table("Directors")]
    public partial class Directors
    {
        public int DirectorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string PictureImg { get; set; } = string.Empty;
        public byte IsActive { get; set; } = 1;

        // Agregar cuando exista la entidad MovieDirectors:
        public virtual ICollection<MovieDirectors> MovieDirectors { get; set; } = new List<MovieDirectors>();
    }
}