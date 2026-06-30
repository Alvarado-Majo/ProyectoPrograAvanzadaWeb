using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Repositories.Actores;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Actors
{
    public class ActorRepository : IActorRepository
    {
        private readonly ProyectoDBContext _context;

        public ActorRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Actors>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Entities.Actors?> GetActorById(int id)
        {
            return await _context.Actors.FirstOrDefaultAsync(a => a.ActorId == id);
        }

        public async Task<Entities.Actors?> GetActorByName(string firstName, string lastName)
        {
            return await _context.Actors.FirstOrDefaultAsync(a =>
                a.FirstName.ToLower() == firstName.ToLower().Trim() &&
                a.LastName.ToLower() == lastName.ToLower().Trim());
        }

        public async Task<List<Entities.Actors>> GetActiveActors(byte isActive)
        {
            return await _context.Actors.Where(a => a.IsActive == isActive).ToListAsync();
        }

        public async Task<List<Entities.Actors?>> GetActorsByMovieId(int movieId)
        {
            return await _context.MovieActors.Where(ma => ma.MovieId == movieId)
                .Select(ma => ma.Actors)
                .ToListAsync();
        }

        public async Task<bool> CreateActor(Entities.Actors actor)
        {
            if (actor == null) return false;

            await _context.Actors.AddAsync(actor);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateActor(Entities.Actors actor)
        {
            if (actor == null) return false;

            var existing = await _context.Actors.FindAsync(actor.ActorId);
            if (existing == null) return false;

            existing.FirstName = actor.FirstName;
            existing.LastName = actor.LastName;
            existing.Nationality = actor.Nationality;
            existing.Biography = actor.Biography;
            existing.BirthDate = actor.BirthDate;
            existing.PictureImg = actor.PictureImg;
            existing.IsActive = actor.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteActor(int id)
        {
            var entity = await _context.Actors.FindAsync(id);
            if (entity == null) return false;

            _context.Actors.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}