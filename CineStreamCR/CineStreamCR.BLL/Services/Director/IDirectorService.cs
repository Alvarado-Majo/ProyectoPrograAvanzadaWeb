using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Director;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Director
{
    public interface IDirectorService
    {
        //crud methods
        Task<List<Respuesta<DirectorDTO>>> GetAllDirectorsAsync();
        Task<Respuesta<DirectorDTO>> GetDirectorByIdAsync(int id);
        Task<Respuesta<DirectorDTO>> GetDirectorByNameAsync(string name);
        Task<Respuesta<DirectorDTO>> GetCreateDirectorAsync(CreateDirectorDTO directorDTO);
        Task<Respuesta<DirectorDTO>> GetUpdateDirectorAsync(CreateDirectorDTO directorDTO);
        Task<Respuesta<DirectorDTO>> GetDeleteDirectorAsync(int id);

        //Additional methods
        Task<List<Respuesta<DirectorDTO>>> GetDirectorsByMovieIdAsync(int movieId);
        Task<List<Respuesta<DirectorDTO>>> GetActiveDirectorsAsync(byte isActive);
    }
}
