using System;
using System.Collections.Generic;
using System.Text;
using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.Movies
{
    public interface IMovieRepository
    {
        Task<List<Entities.Movies>> GetMovies();
        Task<Entities.Movies?> GetMovieById(int id);
        Task<Entities.Movies?> GetMovieDetallesById(int id);
        Task<List<Entities.Movies>> GetMoviesByDirectorId(int directorId);
        Task<List<MovieActors>> GetMoviesByActorId(int actorId);
    }
}