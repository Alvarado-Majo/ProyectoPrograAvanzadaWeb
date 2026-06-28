using CineStreamCR.DAL.Repositories.Actores;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Actors
{
    public class ActorRepository : IActorRepository
    {
        public Task<bool> CreateActorAsync(Entities.Actors actor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteActorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Actors>> GetActiveActorsAsync(byte isActive)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Actors> GetActorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Actors> GetActorByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Actors>> GetActorsByMovieIdAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Actors>> ListActorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateActorAsync(Entities.Actors actor)
        {
            throw new NotImplementedException();
        }
    }
}
