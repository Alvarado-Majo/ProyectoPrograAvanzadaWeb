using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Repositories.Actors
{
    public interface IMovieActorsRepository
    {
        //Crud methods
        Task<Entities.MovieActors?> GetByMovieAndActor(int movieId, int actorId);
        Task<bool> AssignActorToMovie(Entities.MovieActors movieActor);
        Task<bool> RemoveActorFromMovie(int movieId, int actorId);
        Task<bool> UpdateCharacterName(int movieId, int actorId, string characterName);

        //additional methods
    }
}
