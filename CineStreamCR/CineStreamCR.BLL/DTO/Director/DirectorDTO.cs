using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.Director
{
    public class DirectorDTO
    {
        public int DirectorID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; } = null!;
        public string? Nationality { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string PictureImg { get; set; } = null!;
        public byte IsActive { get; set; } = 1;
    }
}
