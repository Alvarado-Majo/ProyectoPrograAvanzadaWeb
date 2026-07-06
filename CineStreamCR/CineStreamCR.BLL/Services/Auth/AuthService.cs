using System;
using System.Collections.Generic;
using CineStreamCR.BLL.DTO.Auth;
using CineStreamCR.DAL.Entities;
using CineStreamCR.DAL.Repositories.Users;

namespace CineStreamCR.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users?> LoginAsync(LoginDTO loginDTO)
        {
            // Usuario temporal para probar login
            if (loginDTO.Email == "admin@gmail.com" && loginDTO.Password == "123")
            {
                return new Users
                {
                    UserId = 1,
                    FirstName = "Admin",
                    LastName = "CineStream",
                    Email = "admin@cinestreamcr.com",
                    IsActive = 1
                };
            }

            return null;
        }

        public async Task<bool> RegisterAsync(Users user)
        {
            return await _userRepository.CreateUser(user);
        }
    }
}