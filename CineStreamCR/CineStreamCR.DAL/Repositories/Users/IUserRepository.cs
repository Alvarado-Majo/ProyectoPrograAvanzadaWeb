using System;
using System.Collections.Generic;
using System.Text;
using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Users
{
    public interface IUserRepository
    {
        Task<List<Entities.Users>> GetUsers();

        Task<Entities.Users?> GetUserById(int id);

        Task<Entities.Users?> GetUserByEmail(string email);

        Task<bool> CreateUser(Entities.Users user);

        Task<bool> UpdateUser(Entities.Users user);

        Task<bool> DeleteUser(int id);
    }
}