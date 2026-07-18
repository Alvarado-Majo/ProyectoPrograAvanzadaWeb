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
                .Include(m => m.MovieDirectors)
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

                .Include(m => m.MovieDirectors)
                    .ThenInclude(md => md.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m =>
                    m.MovieId == id &&
                    m.IsActive == 1);
        }

        public async Task<List<Entities.Movies>> GetMoviesByDirectorId(int directorId)
        {
            return await _context.MovieDirectors
                .Where(md =>
                    md.DirectorId == directorId &&
                    md.Movie.IsActive == 1)
                .Select(md => md.Movie)
                .ToListAsync();
        }

        public async Task<List<Entities.Movies>> GetMoviesByActorId(int actorId)
        {
            return await _context.MovieActors
                .Where(ma =>
                    ma.ActorId == actorId &&
                    ma.Movie.IsActive == 1)
                .Select(ma => ma.Movie)
                .ToListAsync();
        }

        public async Task<bool> CreateMovie(Entities.Movies movie)
        {
            if (movie == null)
                return false;
            await _context.Movies.AddAsync(movie);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMovie(Entities.Movies movie)
        {
            if (movie == null)
                return false;

            var existing = await _context.Movies.FindAsync(movie.MovieId);

            if (existing == null)
                return false;

            existing.Title = movie.Title;
            existing.Synopsis = movie.Synopsis;
            existing.ReleaseYear = movie.ReleaseYear;
            existing.DurationMinutes = movie.DurationMinutes;
            existing.MovieRating = movie.MovieRating;
            existing.PosterImg = movie.PosterImg;
            existing.VideoUrl = movie.VideoUrl;
            existing.Nationality = movie.Nationality;
            existing.UpdatedAt = DateTime.Now;

            _context.Movies.Update(existing);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            movie.IsActive = 0;
            movie.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}