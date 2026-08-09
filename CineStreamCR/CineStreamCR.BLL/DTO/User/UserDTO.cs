using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime SignUpDate { get; set; }
        public byte IsActive { get; set; } 
    }
}
