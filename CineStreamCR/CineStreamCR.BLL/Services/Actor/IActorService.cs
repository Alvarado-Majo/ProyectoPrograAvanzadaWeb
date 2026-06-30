using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Actor
{
    public interface IActorService
    {
        //Crud methods

        Task<List<Answer<ActorDTO>>> GetAllActorsAsync();
        Task<Answer<ActorDTO?>> GetActorByIdAsync(int id);
        Task<Answer<ActorDTO?>> GetActorByNameAsync(string firstName, string lastName);
        Task<Answer<ActorDTO>> GetCreateActorAsync(CreateActorDTO actorDTO);
        Task<Answer<ActorDTO>> GetUpdateActorAsync(int id,CreateActorDTO actorDTO);
        Task<Answer<ActorDTO>> GetDeleteActorAsync(int id);

        //Additional methods
        Task<List<Answer<ActorDTO?>>> GetActorsByMovieIdAsync(int movieId);
        Task<Answer<ActorDTO>> GetActiveActorsAsync(byte isActive);
    }
}
