using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.User;
using CineStreamCR.DAL.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CineStreamCR.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Answer<List<UserDTO>>> GetAllUsers()
        {
            var answer = new Answer<List<UserDTO>>();
            var users = await _userRepository.GetUsers();
            answer.Dato = _mapper.Map<List<UserDTO>>(users);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<UserDTO?>> GetUserById(int id)
        {
            var answer = new Answer<UserDTO?>();
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "User not found.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<UserDTO?>(user);
            return answer;
        }

        public async Task<Answer<UserDTO?>> GetUserByEmail(string email)
        {
            var answer = new Answer<UserDTO?>();
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "User not found.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<UserDTO?>(user);
            return answer;
        }

        public async Task<Answer<UserDTO>> GetCreateUser(CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Invalid user.",
                    codigo = 400
                };
            }

            var existing = await _userRepository.GetUserByEmail(userDTO.Email);
            if (existing != null)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "A user with that email already exists.",
                    codigo = 400
                };
            }

            CreatePasswordHash(userDTO.Password, out byte[] hash, out byte[] salt);

            var newUser = new DAL.Entities.Users
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                SignUpDate = DateTime.Now,
                IsActive = 1
            };

            bool result = await _userRepository.CreateUser(newUser);
            if (!result)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error creating the user.",
                    codigo = 500
                };
            }

            return new Answer<UserDTO>
            {
                EsCorrecto = true,
                mensaje = "User created successfully.",
                Dato = _mapper.Map<UserDTO>(newUser),
                codigo = 201
            };
        }

        public async Task<Answer<UserDTO>> GetUpdateUser(int id, UpdateUserDTO userDTO)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "User not found.",
                    codigo = 404
                };
            }

            var existingWithEmail = await _userRepository.GetUserByEmail(userDTO.Email);
            if (existingWithEmail != null && existingWithEmail.UserId != id)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "A user with that email already exists.",
                    codigo = 400
                };
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.IsActive = userDTO.IsActive;

            if (!string.IsNullOrWhiteSpace(userDTO.Password))
            {
                CreatePasswordHash(userDTO.Password, out byte[] hash, out byte[] salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
            }

            bool result = await _userRepository.UpdateUser(user);
            if (!result)
            {
                return new Answer<UserDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Error updating the user.",
                    codigo = 500
                };
            }

            return new Answer<UserDTO>
            {
                EsCorrecto = true,
                mensaje = "User updated successfully.",
                Dato = _mapper.Map<UserDTO>(user),
                codigo = 200
            };
        }

        public async Task<Answer<bool>> GetDeleteUser(int id)
        {
            var answer = new Answer<bool>();
            bool result = await _userRepository.DeleteUser(id);
            if (!result)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error deleting the user.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "User deleted successfully.";
            answer.codigo = 200;
            answer.Dato = true;
            return answer;
        }

        //hashea la contraseña y genera un salt único para cada usuario
        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
