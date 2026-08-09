using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.User
{
    public interface IUserService
    {
        Task<Answer<List<UserDTO>>> GetAllUsers();

        Task<Answer<UserDTO?>> GetUserById(int id);

        Task<Answer<UserDTO?>> GetUserByEmail(string email);

        Task<Answer<UserDTO>> GetCreateUser(CreateUserDTO userDTO);

        Task<Answer<UserDTO>> GetUpdateUser(int id, UpdateUserDTO userDTO);

        Task<Answer<bool>> GetDeleteUser(int id);
    }
}
