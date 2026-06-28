using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Directors
{
    internal interface IDirectorRepository
    {
        //Crud methods
        Task<List<Entities.Directors>> ListDirectors();
        Task<Entities.Directors> GetDirectorById(int id);
        Task<Entities.Directors> GetDirectorByName(string name);
        Task<bool> CreateDirector(Entities.Directors director);
        Task<bool> UpdateDirector(Entities.Directors director);
        Task<bool> DeleteDirector(int id);

        //additional methods
        Task<List<Entities.Directors>> GetDirectorsByMovieId(int movieId);
        Task<List<Entities.Directors>> GetActiveDirector(byte isActive);
    }
}
