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

        Task<List<Respuesta<ActorDTO>>> GetAllActorsAsync();
        Task<Respuesta<ActorDTO>> GetActorByIdAsync(int id);
        Task<Respuesta<ActorDTO>> GetActorByNameAsync(string name);
        Task<Respuesta<ActorDTO>> GetCreateActorAsync(CreateActorDTO actorDTO);
        Task<Respuesta<ActorDTO>> GetUpdateActorAsync(CreateActorDTO actorDTO);
        Task<Respuesta<ActorDTO>> GetDeleteActorAsync(int id);

        //Additional methods
        Task<List<Respuesta<ActorDTO>>> GetActorsByMovieIdAsync(int movieId);
        Task<List<Respuesta<ActorDTO>>> GetActiveActorsAsync(byte isActive);
    }
}
