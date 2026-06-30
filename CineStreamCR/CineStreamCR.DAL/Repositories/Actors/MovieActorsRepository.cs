using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Actors
{
    public class MovieActorsRepository : IMovieActorsRepository
    {
        private readonly ProyectoDBContext _context;

        public MovieActorsRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<MovieActors?> GetByMovieAndActor(int movieId, int actorId)
        {
            return await _context.MovieActors.FirstOrDefaultAsync(ma =>
                ma.MovieId == movieId &&
                ma.ActorId == actorId);
        }

        //works like a create method
        public async Task<bool> AssignActorToMovie(MovieActors movieActor)
        {
            if (movieActor == null) return false;

            var exists = await GetByMovieAndActor(
                movieActor.MovieId!.Value,
                movieActor.ActorId!.Value);

            if (exists != null) return false;

            await _context.MovieActors.AddAsync(movieActor);
            return await _context.SaveChangesAsync() > 0;
        }

        //works like an delete method
        public async Task<bool> RemoveActorFromMovie(int movieId, int actorId)
        {
            var entity = await GetByMovieAndActor(movieId, actorId);
            if (entity == null) return false;

            _context.MovieActors.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        //works like an update method, but only for the character name
        public async Task<bool> UpdateCharacterName(int movieId, int actorId, string characterName)
        {
            if (string.IsNullOrWhiteSpace(characterName)) return false;

            var entity = await GetByMovieAndActor(movieId, actorId);
            if (entity == null) return false;

            entity.CharacterName = characterName.Trim();
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
