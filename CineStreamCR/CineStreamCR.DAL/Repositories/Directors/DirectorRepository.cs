using CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

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
            var entity = await _context.Directors.FindAsync(id);

            if (entity == null)
                return false;

            entity.IsActive = 0;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Entities.Directors>> GetActiveDirectors(byte isActive)
        {
            return await _context.Directors
                .Where(d => d.IsActive == isActive)
                .ToListAsync();
        }

        public async Task<Entities.Directors?> GetDirectorById(int id)
        {
            return await _context.Directors
                .FirstOrDefaultAsync(d =>
                    d.DirectorId == id &&
                    d.IsActive == 1);
        }

        public async Task<Entities.Directors?> GetDirectorByName(string firstName, string lastName)
        {
            return await _context.Directors.FirstOrDefaultAsync(d =>
                d.FirstName.ToLower() == firstName.ToLower().Trim() &&
                d.LastName.ToLower() == lastName.ToLower().Trim());
        }

        public async Task<List<Entities.Directors>> GetDirectors()
        {
            return await _context.Directors
                .Where(d => d.IsActive == 1)
                .ToListAsync();
        }

        public async Task<List<Entities.Directors>> GetDirectorsByMovieId(int movieId)
        {
            return await _context.MovieDirectors
                .Where(md => md.MovieId == movieId)
                .Select(md => md.Director)
                .ToListAsync();
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
