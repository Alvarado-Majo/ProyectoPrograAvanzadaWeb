using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Director;
using CineStreamCR.DAL.Repositories.Directors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Director
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;

        public DirectorService(IDirectorRepository directorRepository, IMapper mapper)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
        }
        public async Task<Answer<DirectorDTO>> GetActiveDirectorsAsync(byte isActive)
        {
            var answer = new Answer<DirectorDTO>();
            var director = await _directorRepository.GetActiveDirector(isActive);
            if (director == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Director no encontrado";
                answer.codigo = 404;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<DirectorDTO>(director);
            return answer;       
        }

        public async Task<Answer<List<DirectorDTO?>>> GetAllDirectorsAsync()
        {
            var answer = new Answer<List<DirectorDTO?>>();
            var list = await _directorRepository.GetDirectors();
            answer.Dato = _mapper.Map<List<DirectorDTO?>>(list);
            return answer;
        }

        public async Task<Answer<DirectorDTO>> GetCreateDirectorAsync(CreateDirectorDTO directorDTO)
        {
            var answer = new Answer<DirectorDTO>();
            if (directorDTO == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Director Invalido.";
                answer.codigo = 400;
                return answer;
            }

            var directorName = await _directorRepository.GetDirectorByName(directorDTO.FirstName, directorDTO.LastName);
            if (directorName != null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Director ya existe.";
                answer.codigo = 400;
                return answer;
            }

            var newDirector = _mapper.Map<DAL.Entities.Directors>(directorDTO);
            newDirector.IsActive = 1; // Set IsActive to 1 for new directors

            bool result = await _directorRepository.CreateDirector(newDirector);
            if (result)
            {
                return new Answer<DirectorDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Director creado exitosamente.",
                    Dato = _mapper.Map<DirectorDTO>(newDirector),
                    codigo = 201
                };
            }
            return new Answer<DirectorDTO>
            {
                EsCorrecto = false,
                mensaje = "Error al crear el director.",
                codigo = 500
            };
        }

        public async Task<Answer<DirectorDTO>> GetDeleteDirectorAsync(int id)
        {
            var answer = new Answer<DirectorDTO>();
            var result = await _directorRepository.DeleteDirector(id);
            if (result)
            {
                answer.EsCorrecto = true;
                answer.mensaje = "Director eliminado exitosamente.";
                answer.codigo = 200;
                return answer;
            }
        
            answer.EsCorrecto = false;
            answer.mensaje = "Error al eliminar el director.";
            answer.codigo = 500;
            return answer;
            

        }

        public async Task<Answer<DirectorDTO?>> GetDirectorByIdAsync(int id)
        {
            var answer = new Answer<DirectorDTO?>();
            var director = await _directorRepository.GetDirectorById(id);
            if (director == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Director not found";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<DirectorDTO?>(director);
            return answer;
        }

        public async Task<Answer<DirectorDTO?>> GetDirectorByNameAsync(string firstName, string lastName)
        {
            var answer = new Answer<DirectorDTO?>();
            var director = await _directorRepository.GetDirectorByName(firstName, lastName);
            if(director == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Director not found";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<DirectorDTO?>(director);
            return answer;
        }

        public async Task<List<Answer<DirectorDTO?>>> GetDirectorsByMovieIdAsync(int movieId)
        {
            var directors = await _directorRepository.GetDirectorsByMovieId(movieId);
            var answers = new List<Answer<DirectorDTO?>>();
            foreach (var director in directors)
            {
                answers.Add(new Answer<DirectorDTO?>
                {
                    EsCorrecto = true,
                    Dato = _mapper.Map<DirectorDTO?>(director)
                });
            }
            return answers;
        }

        public async Task<Answer<DirectorDTO>> GetUpdateDirectorAsync(int id, CreateDirectorDTO directorDTO)
        {
            var answer = await _directorRepository.GetDirectorById(id);
            if (answer == null)
            {
                return new Answer<DirectorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Director not found",
                    codigo = 404
                };
            }
            var directorName = await _directorRepository.GetDirectorByName(directorDTO.FirstName, directorDTO.LastName);
            if (directorName != null && directorName.DirectorId != id)
            {
                return new Answer<DirectorDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Director with the same name already exists",
                    codigo = 400
                };
            }
            answer.FirstName = directorDTO.FirstName;
            answer.LastName = directorDTO.LastName;
            answer.Biography = directorDTO.Biography;
            answer.BirthDate = directorDTO.BirthDate.HasValue ? DateOnly.FromDateTime(directorDTO.BirthDate.Value) : null;
            answer.PictureImg = directorDTO.PinctureImg;
            answer.IsActive = directorDTO.IsActive;

            bool result = await _directorRepository.UpdateDirector(answer);

            if (result)
            {
                return new Answer<DirectorDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Director updated successfully",
                    Dato = _mapper.Map<DirectorDTO>(answer),
                    codigo = 200
                };
            }
            return new Answer<DirectorDTO>
            {
                EsCorrecto = false,
                mensaje = "Error updating director",
                codigo = 500
            };
        }
    }
}


