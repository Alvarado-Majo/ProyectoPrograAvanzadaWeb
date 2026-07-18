using CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Repositories.WatchLists
{
    public class WatchListRepository : IWatchListRepository
    {
        private readonly ProyectoDBContext _context;

        public WatchListRepository(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.WatchLists>> GetWatchLists()
        {
            return await _context.WatchLists
                .ToListAsync();
        }

        public async Task<Entities.WatchLists?> GetWatchListById(int id)
        {
            return await _context.WatchLists
                .FirstOrDefaultAsync(w =>
                    w.WatchListId == id);
        }

        public async Task<List<Entities.WatchLists>> GetWatchListsByUserId(int userId)
        {
            return await _context.WatchLists
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CreateWatchList(
            Entities.WatchLists watchList)
        {
            if (watchList == null)
                return false;

            await _context.WatchLists
                .AddAsync(watchList);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateWatchList(Entities.WatchLists watchList)
        {
            if (watchList == null)
                return false;

            var existingWatchList =
                await _context.WatchLists
                    .FindAsync(watchList.WatchListId);

            if (existingWatchList == null)
                return false;

            existingWatchList.Name =
                watchList.Name;

            existingWatchList.Description =
                watchList.Description;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWatchList(int id)
        {
            var entity =
                await _context.WatchLists
                    .FindAsync(id);

            if (entity == null)
                return false;

            _context.WatchLists.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}