using Delwings.Context;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Delwings.Models.Basic;

namespace Delwings.Repositories
{
    public class PlaceRepository: IPlaceRepository
    {
        public readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context) { _context = context; }

        public async Task<List<Place>> GetAllPlacesAsync() => await _context.Set<Place>().ToListAsync(); 

        public async Task<Place?> GetPlaceByIdAsync(int id)
        {
            return await _context.Set<Place>().FirstOrDefaultAsync(place => place.Id == id);
        }

        public async Task CreatePlaceAsync(Place place)
        {
            await _context.Set<Place>().AddAsync(place);
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            _context.Set<Place>().Update(place);
        }

        public async Task DeletePlaceAsync(Place place)
        {
            _context.Set<Place>().Remove(place);
        }

        public async Task ClearPlaces() => await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Places");
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
