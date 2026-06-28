using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Actors
{
    public interface IMovieActorsRepository
    {
        //Crud methods
        public Task<List<Entities.MovieActors>> ListMovieActors();
        public Task<Entities.MovieActors> GetMovieActorById(int id);
        public Task<bool> CreateMovieActor(Entities.MovieActors movieActors);
        public Task<bool> UpdateMovieActor(Entities.MovieActors movieActors);
        public Task<bool> DeleteMovieActor(int id);

        //additional methods
    }
}
