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
        public DateTime? BirthDate { get; set; }
        public string PinctureImg { get; set; } = null!;

    }
}
