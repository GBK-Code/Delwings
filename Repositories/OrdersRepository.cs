using Delwings.Context;
using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context) { _context = context; }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Set<Order>().ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id) => await _context.Set<Order>().FirstOrDefaultAsync(order => order.Id == id);
        public async Task<Order?> GetOrderByTrackIdAsync(string trackId) =>
            await _context.Set<Order>().FirstOrDefaultAsync(order => order.TrackId == trackId);

        public async Task<Order?> GetOrderByReceiverNumberAsync(int receiverNumber) =>
            await _context.Set<Order>().FirstOrDefaultAsync(order => order.ReceiverNumber == receiverNumber);
        public async Task AddOrderAsync(Order order)
        {
            var dbSet = _context.Set<Order>();
            await dbSet.AddAsync(order);
        }
        public Task UpdateOrderAsync(Order order)
        {
            _context.Set<Order>().Update(order);
            return Task.CompletedTask;
        }
        public Task DeleteOrderAsync(Order order)
        {
            _context.Set<Order>().Remove(order);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
