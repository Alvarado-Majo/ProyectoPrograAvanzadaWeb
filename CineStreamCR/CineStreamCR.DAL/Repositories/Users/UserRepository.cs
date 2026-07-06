using System;
using System.Collections.Generic;
using System.Text;
using CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ProyectoDBContext _context;

        public UserRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Users>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Entities.Users?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<Entities.Users?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower().Trim());
        }

        public async Task<bool> CreateUser(Entities.Users user)
        {
            if (user == null) return false;

            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUser(Entities.Users user)
        {
            if (user == null) return false;

            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser == null) return false;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.PasswordSalt = user.PasswordSalt;
            existingUser.IsActive = user.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return false;

            _context.Users.Remove(user);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}