using Delwings.Models;
using Delwings.Context;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class OperatorPlacesRepository : IOperatorPlacesRepository
    {
        private readonly AppDbContext _context;

        public OperatorPlacesRepository(AppDbContext context) { _context = context; }

        public async Task<List<OperatorPlace>> GetAllOperatorPlacesAsync() => await _context.Set<OperatorPlace>().ToListAsync();

        public async Task<OperatorPlace?> GetOperatorPlaceByIdAsync(int id) =>
            await _context.Set<OperatorPlace>().FirstOrDefaultAsync(place => place.Id == id);

        public async Task<OperatorPlace?> GetOperatorPlaceByOperatorIdAsync(int operatorId) =>
            await _context.Set<OperatorPlace>().FirstOrDefaultAsync(place => place.OperatorId == operatorId);

        public async Task<OperatorPlace?> GetOperatorPlaceByPlaceIdAsync(int placeId) =>
            await _context.Set<OperatorPlace>().FirstOrDefaultAsync(place => place.PlaceId == placeId);

        public async Task CreateOperatorPlaceAsync(OperatorPlace newPlace)
        {
            await _context.Set<OperatorPlace>().AddAsync(newPlace);
        }

        public Task UpdateOperatorPlace(OperatorPlace newData)
        {
             _context.Set<OperatorPlace>().Update(newData);
            return Task.CompletedTask;
        }

        public Task DeleteOperatorPlace(OperatorPlace place)
        {
            _context.Set<OperatorPlace>().Remove(place);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
