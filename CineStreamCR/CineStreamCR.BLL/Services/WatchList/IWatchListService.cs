using CineStreamCR.BLL.DTO;
using CineStreamCR.BLL.DTO.WatchList;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.Services.WatchList
{
    public interface IWatchListService
    {
        Task<Answer<List<WatchListDTO>>> GetWatchLists();

        Task<Answer<WatchListDTO?>> GetWatchListById(int id);

        Task<Answer<WatchListDTO>> CreateWatchList(CreateWatchListDTO watchList);

        Task<Answer<WatchListDTO>> UpdateWatchList(WatchListDTO watchList);

        Task<Answer<bool>> DeleteWatchList(int id);

        // Consultas

        Task<Answer<List<WatchListDTO>>> GetWatchListsByUserId(int userId);
    }
}
