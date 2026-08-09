using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.DTO.WatchList;
using CineStreamCR.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.WatchList
{
    public interface IWatchListMovieService
    {

        Task<Answer<WatchListMovieDTO?>> GetByWatchListAndMovie(int watchListId, int movieId);
        Task<Answer<WatchListMovieDTO?>> AddMovieToWatchList(WatchListMovieDTO watchListMovie);
        Task<Answer<bool>> RemoveMovieFromWatchList(int watchListId, int movieId);

        // Consultas
        Task<Answer<List<WatchListMovieDTO>>> GetByWatchListId(int watchListId);
        Task<Answer<List<WatchListMovieDTO>>> GetByMovieId(int movieId);
    }
}
