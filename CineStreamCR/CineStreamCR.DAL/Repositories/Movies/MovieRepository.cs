using System;
using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.Movies
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ProyectoDBContext _context;

        public MovieRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Movies>> GetMovies()
        {
            return await _context.Movies
                .Include(m => m.Director)
                .Where(m => m.IsActive == 1)
                .ToListAsync();
        }

        public async Task<Entities.Movies?> GetMovieById(int id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
        }

        public async Task<Entities.Movies?> GetMovieDetallesById(int id)
        {
            return await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actors)
                .FirstOrDefaultAsync(m => m.MovieId == id && m.IsActive == 1);
        }

        public async Task<List<Entities.Movies>> GetMoviesByDirectorId(int directorId)
        {
            return await _context.Movies
                .Where(m => m.DirectorId == directorId && m.IsActive == 1)
                .ToListAsync();
        }

        public async Task<List<MovieActors>> GetMoviesByActorId(int actorId)
        {
            return await _context.MovieActors
                .Include(ma => ma.Movie)
                .Where(ma => ma.ActorId == actorId && ma.Movie.IsActive == 1)
                .ToListAsync();
        }
    }
}