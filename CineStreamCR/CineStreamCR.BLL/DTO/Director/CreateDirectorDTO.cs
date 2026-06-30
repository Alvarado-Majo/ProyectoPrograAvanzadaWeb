using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CineStreamCR.BLL.DTO.Director
{
    public class CreateDirectorDTO
    {
        [Key]
        public int DirectorId { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Biography is required.")]
        public string Biography { get; set; } = null!;
        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Picture image is required.")]
        public string PinctureImg { get; set; } = null!;
        [Required(ErrorMessage = "The status is required.")]
        public byte IsActive { get; set; } = 1;
    }
}
