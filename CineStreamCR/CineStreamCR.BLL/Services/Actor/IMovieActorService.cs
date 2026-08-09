using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Actor;
using CineStreamCR.BLL.DTO.Movie;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.Actor
{
    public interface IMovieActorService
    {
        //CRUD
        Task<Answer<MovieCastMemberDTO?>> AssignActorToMovie(AssignActorToMovieDTO dto);
        Task<Answer<bool>> RemoveActorFromMovie(int movieId, int actorId);
        Task<Answer<MovieCastMemberDTO?>> UpdateCharacterName(int movieId, int actorId, string characterName);

        // Consultas
        Task<Answer<List<MovieCastMemberDTO>>> GetActorsByMovieId(int movieId);
        Task<Answer<List<MovieCastMemberDTO>>> GetMoviesByActorId(int actorId);
    }
}
