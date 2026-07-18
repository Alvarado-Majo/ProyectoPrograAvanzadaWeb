using CineStreamCR.DAL.Entities;

namespace CineStreamCR.DAL.Repositories.WatchLists
{
    public interface IWatchListRepository
    {
        Task<List<Entities.WatchLists>> GetWatchLists();

        Task<Entities.WatchLists?> GetWatchListById(int id);

        Task<bool> CreateWatchList(Entities.WatchLists watchList);

        Task<bool> UpdateWatchList(Entities.WatchLists watchList);

        Task<bool> DeleteWatchList(int id);

        // Consultas

        Task<List<Entities.WatchLists>> GetWatchListsByUserId(int userId);
    }
}