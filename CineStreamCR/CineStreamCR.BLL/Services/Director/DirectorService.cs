using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Director;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Director
{
    public class DirectorService : IDirectorService
    {
        public Task<List<Respuesta<DirectorDTO>>> GetActiveDirectorsAsync(byte isActive)
        {
            throw new NotImplementedException();
        }

        public Task<List<Respuesta<DirectorDTO>>> GetAllDirectorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<DirectorDTO>> GetCreateDirectorAsync(CreateDirectorDTO directorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<DirectorDTO>> GetDeleteDirectorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<DirectorDTO>> GetDirectorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<DirectorDTO>> GetDirectorByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Respuesta<DirectorDTO>>> GetDirectorsByMovieIdAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<DirectorDTO>> GetUpdateDirectorAsync(CreateDirectorDTO directorDTO)
        {
            throw new NotImplementedException();
        }
    }
}
