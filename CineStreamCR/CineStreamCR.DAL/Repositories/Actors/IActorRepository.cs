

namespace CineStreamCR.DAL.Repositories.Actores
{
    public interface IActorRepository
    {
        //Crud methods
        Task<List<Entities.Actors>> GetActors();
        Task<Entities.Actors?> GetActorById(int id);
        Task<Entities.Actors?> GetActorByName(string firstName, string lastName);
        Task<bool> CreateActor(Entities.Actors actor);
        Task<bool> UpdateActor(Entities.Actors actor);
        Task<bool> DeleteActor(int id);


        //additional methods
        Task<List<Entities.Actors?>> GetActorsByMovieId(int movieId);
        Task<List<Entities.Actors>> GetActiveActors(byte isActive);
    }
}
