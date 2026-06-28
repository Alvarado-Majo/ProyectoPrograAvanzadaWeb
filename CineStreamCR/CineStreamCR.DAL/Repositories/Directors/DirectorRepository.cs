using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Directors
{
    public class DirectorRepository : IDirectorRepository
    {
        public Task<bool> CreateDirectorAsync(Entities.Directors director)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDirectorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Directors>> GetActiveDirectorsAsync(byte isActive)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Directors> GetDirectorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Directors> GetDirectorByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Directors>> GetDirectorsByMovieIdAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Directors>> ListDirectorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDirectorAsync(Entities.Directors director)
        {
            throw new NotImplementedException();
        }
    }
}
