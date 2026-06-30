using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.DAL.Repositories.Actores;
using CineStreamCR.DAL.Repositories.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Actor
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public async Task<List<Answer<ActorDTO>>> GetAllActorsAsync()
        {
            var actors = await _actorRepository.GetActors();
            var answers = new List<Answer<ActorDTO>>();
            foreach (var actor in actors)
            {
                answers.Add(new Answer<ActorDTO>
                {
                    EsCorrecto = true,
                    Dato = _mapper.Map<ActorDTO>(actor)
                });
            }
            return answers;
        }

        public async Task<Answer<ActorDTO>> GetActiveActorsAsync(byte isActive)
        {
            var answer = new Answer<ActorDTO>();
            var actor = await _actorRepository.GetActiveActors(isActive);
            if (actor == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Actor no encontrado";
                answer.codigo = 404;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<ActorDTO>(actor);
            return answer;
        }

        public async Task<Answer<ActorDTO>> GetActorByIdAsync(int id)
        {
            var answer = new Answer<ActorDTO>();
            var actor = await _actorRepository.GetActorById(id);
            if (actor == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Actor no encontrado.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<ActorDTO>(actor);
            return answer;
        }

        public async Task<Answer<ActorDTO>> GetActorByNameAsync(string firstName, string lastName)
        {
     
            var answer = new Answer<ActorDTO>();
            var actor = await _actorRepository.GetActorByName(firstName, lastName);
            if (actor == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Actor not found";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<ActorDTO>(actor);
            return answer;
        }

        public async Task<List<Answer<ActorDTO>>> GetActorsByMovieIdAsync(int movieId)
        {
            var actors = await _actorRepository.GetActorsByMovieId(movieId);
            var answers = new List<Answer<ActorDTO>>();
            foreach (var actor in actors)
            {
                answers.Add(new Answer<ActorDTO>
                {
                    EsCorrecto = true,
                    Dato = _mapper.Map<ActorDTO>(actor)
                });
            }
            return answers;
        }

        public async Task<Answer<ActorDTO>> GetCreateActorAsync(CreateActorDTO actorDTO)
        {
            if (actorDTO == null)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Actor inválido.",
                    codigo = 400
                };
            }

            var existing = await _actorRepository.GetActorByName(actorDTO.FirstName, actorDTO.LastName);
            if (existing != null)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "El actor ya existe.",
                    codigo = 400
                };
            }

            var newActor = _mapper.Map<DAL.Entities.Actors>(actorDTO);
            newActor.IsActive = 1;

            bool result = await _actorRepository.CreateActor(newActor);
            if (result)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Actor creado exitosamente.",
                    Dato = _mapper.Map<ActorDTO>(newActor),
                    codigo = 201
                };
            }
            return new Answer<ActorDTO>
            {
                EsCorrecto = false,
                mensaje = "Error al crear el actor.",
                codigo = 500
            };
        }

        public async Task<Answer<ActorDTO>> GetUpdateActorAsync(int id,CreateActorDTO actorDTO)
        {
            var answer = await _actorRepository.GetActorById(id);
            if (answer == null)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Actor not found",
                    codigo = 404
                };
            }
            var actorName = await _actorRepository.GetActorByName(actorDTO.FirstName, actorDTO.LastName);
            if (actorName != null && actorName.ActorId != id)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Actor with the same name already exists",
                    codigo = 400
                };
            }
            answer.FirstName = actorDTO.FirstName;
            answer.LastName = actorDTO.LastName;
            answer.Biography = actorDTO.Biography;
            answer.BirthDate = actorDTO.BirthDate.HasValue ? DateOnly.FromDateTime(actorDTO.BirthDate.Value) : null;
            answer.PictureImg = actorDTO.PinctureImg;
            answer.IsActive = actorDTO.IsActive;

            bool result = await _actorRepository.UpdateActor(answer);

            if (result)
            {
                return new Answer<ActorDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Actor updated successfully",
                    Dato = _mapper.Map<ActorDTO>(answer),
                    codigo = 200
                };
            }
            return new Answer<ActorDTO>
            {
                EsCorrecto = false,
                mensaje = "Error updating actor",
                codigo = 500
            };
        
       }

        public async Task<Answer<ActorDTO>> GetDeleteActorAsync(int id)
        {
            var answer = new Answer<ActorDTO>();
            bool result = await _actorRepository.DeleteActor(id);
            if (result)
            {
                answer.EsCorrecto = true;
                answer.mensaje = "Actor eliminado exitosamente.";
                answer.codigo = 200;
                return answer;
            }
            answer.EsCorrecto = false;
            answer.mensaje = "Error al eliminar el actor.";
            answer.codigo = 500;
            return answer;
        }
    }
}