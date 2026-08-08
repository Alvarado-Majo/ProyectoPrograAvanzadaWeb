using AutoMapper;
using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.WatchList;
using CineStreamCR.DAL.Entities;
using CineStreamCR.DAL.Repositories.WatchLists;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.WatchList
{
    public class WatchListService : IWatchListService
    {
        private readonly IWatchListRepository _watchListRepository;
        private readonly IWatchListMoviesRepository _watchListMoviesRepository;
        private readonly IMapper _mapper;

        public WatchListService(IWatchListRepository watchListRepository, IWatchListMoviesRepository watchListMoviesRepository, IMapper mapper)
        {
            _watchListRepository = watchListRepository;
            _watchListMoviesRepository = watchListMoviesRepository;
            _mapper = mapper;
        }

        public async Task<Answer<WatchListDTO>> CreateWatchList(CreateWatchListDTO watchList)
        {
            var answer = new Answer<WatchListDTO>();

            var usuarioWishList = await _watchListRepository.GetWatchListsByUserId(watchList.UserId);
            if (!usuarioWishList.Any())
            {
                var newWatchList = _mapper.Map<WatchLists>(watchList);
                var createdWatchList = await _watchListRepository.CreateWatchList(newWatchList);
                answer.EsCorrecto = true;
                answer.mensaje = "Watchlist created successfully.";
                answer.Dato = _mapper.Map<WatchListDTO>(createdWatchList);
            }
            else
            {
                answer.EsCorrecto = false;
                answer.mensaje = "User already has a watchlist.";
                answer.Dato = _mapper.Map<WatchListDTO>(usuarioWishList.First());
            }

            return answer;
        }

        public async Task<Answer<bool>> DeleteWatchList(int id)
        {
            var answer = new Answer<bool>();

            var watchList = await _watchListRepository.GetWatchListById(id);
            if (watchList == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Watchlist not found.";
                return answer;
            }

            var moviesInList = await _watchListMoviesRepository.GetByWatchListId(id);
            var emptyList = moviesInList.Count() == 0;
            if (!emptyList)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Cannot delete a watchlist that has movies.";
                return answer;
            }

            await _watchListRepository.DeleteWatchList(id);
            answer.EsCorrecto = true;
            answer.mensaje = "Watchlist deleted successfully.";
            return answer;
        }

        public async Task<Answer<WatchListDTO?>> GetWatchListById(int id)
        {
            var answer = new Answer<WatchListDTO?>();
            var watchList = await _watchListRepository.GetWatchListById(id);
            if (watchList == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Watchlist not found.";
                return answer;
            }
            answer.EsCorrecto = true;
            answer.Dato = _mapper.Map<WatchListDTO?>(watchList);
            return answer;
        }

        public async Task<Answer<List<WatchListDTO>>> GetWatchLists()
        {
            var answer = new Answer<List<WatchListDTO>>();
            var watchLists = await _watchListRepository.GetWatchLists();
            answer.Dato = _mapper.Map<List<WatchListDTO>>(watchLists);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<List<WatchListDTO>>> GetWatchListsByUserId(int userId)
        {
            var answer = new Answer<List<WatchListDTO>>();
            var watchLists = await _watchListRepository.GetWatchListsByUserId(userId);
            answer.Dato = _mapper.Map<List<WatchListDTO>>(watchLists);
            answer.EsCorrecto = true;
            return answer;
        }

        public async Task<Answer<WatchListDTO>> UpdateWatchList(WatchListDTO watchList)
        {
           var answer = new Answer<WatchListDTO>();
            var existingWatchList = await _watchListRepository.GetWatchListById(watchList.WatchListId);
            if (existingWatchList == null)
            {
                answer.EsCorrecto = false;
                answer.mensaje = "Watchlist not found.";
                return answer;
            }
            // Update the watchlist properties
            existingWatchList.Name = watchList.Name;
            existingWatchList.UserId = watchList.UserId;
            var updatedWatchList = await _watchListRepository.UpdateWatchList(existingWatchList);
            answer.EsCorrecto = true;
            answer.mensaje = "Watchlist updated successfully.";
            answer.Dato = _mapper.Map<WatchListDTO>(updatedWatchList);
            return answer;
        }
    }
}
