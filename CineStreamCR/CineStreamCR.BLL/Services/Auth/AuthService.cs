using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
            // Buscar usuario por correo
            var user = await _userRepository.GetUserByEmail(loginDTO.Email);

            if (user == null)
                return null;

            // Verificar que el usuario esté activo
            if (user.IsActive != 1)
                return null;

            // Generar el hash de la contraseña ingresada
            // usando el mismo salt guardado en la BD
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(loginDTO.Password)
            );

            // Comparar el hash calculado con el guardado
            if (!CryptographicOperations.FixedTimeEquals(
                    computedHash,
                    user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            // Check if email already exists
            var existingUser =
                await _userRepository.GetUserByEmail(registerDTO.Email);

            if (existingUser != null)
                return false;

            // Generate password salt and hash
            using var hmac = new HMACSHA512();

            var user = new Users
            {
                FirstName = registerDTO.FirstName.Trim(),
                LastName = registerDTO.LastName.Trim(),
                Email = registerDTO.Email.Trim().ToLower(),

                PasswordSalt = hmac.Key,

                PasswordHash = hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(registerDTO.Password)
                ),

                SignUpDate = DateTime.Now,
                IsActive = 1
            };

            return await _userRepository.CreateUser(user);
        }
    }
}