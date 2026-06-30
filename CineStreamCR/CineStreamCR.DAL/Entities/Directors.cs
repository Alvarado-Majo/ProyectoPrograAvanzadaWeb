using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CineStreamCR.DAL.Entities
{
    [Table("Directors")]
    public class Directors
    {
        public int DirectorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string PictureImg { get; set; } = string.Empty;
        public byte IsActive { get; set; } = 1;

        //public virtual ICollection<Movies> Movies { get; set; } = new List<Movies>();
    }
}
