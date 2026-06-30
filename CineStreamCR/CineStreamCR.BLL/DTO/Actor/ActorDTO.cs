using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.Actor
{
    public class ActorDTO
    {
        public int ActorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string PinctureImg { get; set; } = null!;

        


    }
}
