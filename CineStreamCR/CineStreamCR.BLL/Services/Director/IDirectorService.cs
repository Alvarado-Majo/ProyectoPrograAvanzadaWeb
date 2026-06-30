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
        Task<Answer<List<DirectorDTO?>>> GetAllDirectorsAsync();
        Task<Answer<DirectorDTO?>> GetDirectorByIdAsync(int id);
        Task<Answer<DirectorDTO?>> GetDirectorByNameAsync(string firstName, string lastName);
        Task<Answer<DirectorDTO>> GetCreateDirectorAsync(CreateDirectorDTO directorDTO);
        Task<Answer<DirectorDTO>> GetUpdateDirectorAsync(int id, CreateDirectorDTO directorDTO);
        Task<Answer<DirectorDTO>> GetDeleteDirectorAsync(int id);

        //Additional methods
        Task<List<Answer<DirectorDTO?>>> GetDirectorsByMovieIdAsync(int movieId);
        Task<Answer<DirectorDTO>> GetActiveDirectorsAsync(byte isActive);
       
    }
}
