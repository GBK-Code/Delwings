using Delwings.Context;
using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class OrdersHistoryRepository: IOrdersHistoryRepository
    {
        private readonly AppDbContext _context;

        public OrdersHistoryRepository(AppDbContext context) { _context = context; }

        public async Task<List<OrdersHistory>> GetAllHistoryAsync() => await _context.Set<OrdersHistory>().ToListAsync();
        public async Task<OrdersHistory?> GetRecordByIdAsync(int id) => await _context.Set<OrdersHistory>().FirstOrDefaultAsync(rec => rec.Id == id);
        public async Task CreateRecordAsync(OrdersHistory orderRecord)
        {
            await _context.Set<OrdersHistory>().AddAsync(orderRecord);
        }
        public Task UpdateRecordAsync(OrdersHistory orderRecord)
        {
            _context.Set<OrdersHistory>().Update(orderRecord);
            return Task.CompletedTask;
        }
        public Task DeleteRecordAsync(OrdersHistory orderRecord)
        {
            _context.Set<OrdersHistory>().Remove(orderRecord);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
