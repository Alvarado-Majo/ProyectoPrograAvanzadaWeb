using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.Movie;
using CineStreamCR.BLL.DTO.WatchList;
using CineStreamCR.DAL.Repositories.WatchLists;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.WatchList
{
    public class WatchListMovieService : IWatchListMovieService
    {
        private readonly IWatchListMoviesRepository _watchListMovieRepository;
        private readonly IWatchListRepository _watchListRepository;
        private readonly IMapper _mapper;

        public WatchListMovieService(IWatchListMoviesRepository watchListMovieRepository, IWatchListRepository watchListRepository, IMapper mapper)
        {
            _watchListMovieRepository = watchListMovieRepository;
            _watchListRepository = watchListRepository;
            _mapper = mapper;
        }
        public async Task<Answer<WatchListMovieDTO?>> AddMovieToWatchList(WatchListMovieDTO watchListMovie)
        {
            var answer = new Answer<WatchListMovieDTO?>();

            // 1. Confirmar que la wishlist existe
            var watchList = await _watchListRepository.GetWatchListById(watchListMovie.WatchListId);
            if (watchList == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "No se encontró la lista de deseos.";
                return answer;
            }

            // 2. Comprobar si la película ya está en esa wishlist
            var existingMovie = await _watchListMovieRepository.GetByWatchListAndMovie(watchListMovie.WatchListId, watchListMovie.MovieId);
            if (existingMovie != null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "La película ya está en la lista de deseos.";
                return answer;
            }

            // 3. Añadirla
            var newWatchListMovie = new DAL.Entities.WatchListMovies
            {
                WatchListId = watchListMovie.WatchListId,
                MovieId = watchListMovie.MovieId
            };

            await _watchListMovieRepository.AddMovieToWatchList(newWatchListMovie);

            answer.Dato = _mapper.Map<WatchListMovieDTO>(newWatchListMovie);
            answer.EsCorrecto = true;
            answer.mensaje = "Película añadida a la lista de deseos.";
            return answer;
        }
        public async Task<Answer<List<WatchListMovieDTO>>> GetByMovieId(int movieId)
        {
            var answer = new Answer<List<WatchListMovieDTO>>();
            var watchListMovies = await _watchListMovieRepository.GetByMovieId(movieId);
            answer.EsCorrecto = true;
            answer.mensaje = "Consulta realizada correctamente.";
            answer.Dato = _mapper.Map<List<WatchListMovieDTO>>(watchListMovies);
            return answer;
        }

        public async Task<Answer<WatchListMovieDTO?>> GetByWatchListAndMovie(int watchListId, int movieId)
        {
            var answer = new Answer<WatchListMovieDTO?>();
            var watchListMovie = await _watchListMovieRepository.GetByWatchListAndMovie(watchListId, movieId);
            if (watchListMovie == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "No se encontró la película en la lista.";
                answer.codigo = 404;
                return answer;
            }
            answer.EsCorrecto = true;
            answer.mensaje = "Película encontrada en la lista.";
            answer.Dato = _mapper.Map<WatchListMovieDTO?>(watchListMovie);
            return answer;
        }

        public async Task<Answer<List<WatchListMovieDTO>>> GetByWatchListId(int watchListId)
        {
            var answer = new Answer<List<WatchListMovieDTO>>();
            var lista = await _watchListMovieRepository.GetByWatchListId(watchListId);
            answer.Dato = _mapper.Map<List<WatchListMovieDTO>>(lista);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<bool>> RemoveMovieFromWatchList(int watchListId, int movieId)
        {
            var answer = new Answer<bool>();
            var watchListMovie = await _watchListMovieRepository.GetByWatchListAndMovie(watchListId, movieId);
            if (watchListMovie == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "No se encontró la película en la lista de deseos.";
                answer.codigo = 404;
                return answer;
            }

            var removed = await _watchListMovieRepository.RemoveMovieFromWatchList(watchListId, movieId);
            if (!removed)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Error al eliminar la película de la lista de deseos.";
                answer.codigo = 500;
                return answer;
            }

            answer.EsCorrecto = true;
            answer.mensaje = "Película eliminada de la lista de deseos.";
            answer.codigo = 200;
            answer.Dato = true;
            return answer;
        }
    }
}
