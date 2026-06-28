using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Actor
{
    public class ActorService : IActorService
    {
        public Task<List<Respuesta<ActorDTO>>> GetActiveActorsAsync(byte isActive)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<ActorDTO>> GetActorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<ActorDTO>> GetActorByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Respuesta<ActorDTO>>> GetActorsByMovieIdAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Respuesta<ActorDTO>>> GetAllActorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<ActorDTO>> GetCreateActorAsync(CreateActorDTO actorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<ActorDTO>> GetDeleteActorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<ActorDTO>> GetUpdateActorAsync(CreateActorDTO actorDTO)
        {
            throw new NotImplementedException();
        }
    }
}
