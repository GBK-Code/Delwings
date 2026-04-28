using Delwings.Context;
using Delwings.Models;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class CourierOrdersRepository: ICourierOrdersRepository
    {
        private readonly AppDbContext _context;

        public CourierOrdersRepository(AppDbContext context) { _context = context; }

        public async Task<List<CourierOrder>> GetAllCourierOrdersAsync() => await _context.Set<CourierOrder>().ToListAsync();
        public async Task<CourierOrder?> GetCourierOrderByIdAsync(int id) => _context.Set<CourierOrder>().FirstOrDefault(ord => ord.Id == id);
        public async Task<CourierOrder?> GetCourierOrderByCourierIdAsync(int id) => _context.Set<CourierOrder>().FirstOrDefault(ord => ord.CourierId == id);
        public async Task<CourierOrder?> GetCourierOrderByOrderIdAsync(int orderId) => _context.Set<CourierOrder>().FirstOrDefault(ord => ord.CourierId == orderId);
        public async Task<CourierOrder?> GetCourierOrderByToPlaceIdAsync(int placeId) => _context.Set<CourierOrder>().FirstOrDefault(ord => ord.ToPlaceId == placeId);
        public async Task CreateCourierOrderAsync(CourierOrder order) => await _context.Set<CourierOrder>().AddAsync(order);
        public Task UpdateCourierOrderAsync(CourierOrder order)
        {
            _context.Set<CourierOrder>().Update(order);
            return Task.CompletedTask;
        }
        public Task DeleteCourierOrderAsync(CourierOrder order)
        {
            _context.Set<CourierOrder>().Remove(order);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
