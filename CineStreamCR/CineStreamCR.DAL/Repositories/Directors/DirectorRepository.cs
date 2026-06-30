using CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Directors
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly ProyectoDBContext _context;

        public DirectorRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateDirector(Entities.Directors director)
        {
            if (director == null) return false;

            await _context.Directors.AddAsync(director);
            return await _context.SaveChangesAsync()>0;

        }

        public async Task<bool> DeleteDirector(int id)
        {
            var entity = _context.Directors.Find(id);
            if (entity == null) return false;

            _context.Directors.Remove(entity);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<List<Entities.Directors?>> GetActiveDirector(byte isActive)
        {
            return await _context.Directors.Where(d => d.IsActive == isActive).ToListAsync();
        }

        public async Task<Entities.Directors?> GetDirectorById(int id)
        {
            return await _context.Directors.FirstOrDefaultAsync(d => d.DirectorId == id);
        }

        public async Task<Entities.Directors?> GetDirectorByName(string firstName, string lastName)
        {
            return await _context.Directors.FirstOrDefaultAsync(d =>
                d.FirstName.ToLower() == firstName.ToLower().Trim() &&
                d.LastName.ToLower() == lastName.ToLower().Trim());
        }

        public async Task<List<Entities.Directors>> GetDirectors()
        {
            return await _context.Directors.ToListAsync();
        }

        public async Task<List<Entities.Directors>> GetDirectorsByMovieId(int movieId)
        {
            //return await _context.Directors
            //.Where(d => d.Movies.Any(m => m.MovieId == movieId))
            // .ToListAsync();
            throw new NotImplementedException();
        }

       
        public async Task<bool> UpdateDirector(Entities.Directors director)
        {
            if(director == null) return false;
            var existingDirector = await _context.Directors.FindAsync(director.DirectorId);
            if(existingDirector == null) return false;

            // Update the existing director with the new values
            existingDirector.FirstName = director.FirstName;
            existingDirector.LastName = director.LastName;
            existingDirector.Nationality = director.Nationality;
            existingDirector.Biography = director.Biography;
            existingDirector.BirthDate = director.BirthDate;
            existingDirector.PictureImg = director.PictureImg;
            existingDirector.IsActive = director.IsActive;
            
            return await _context.SaveChangesAsync()>0;
        }
    }
    
}
