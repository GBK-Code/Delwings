using Delwings.Context;
using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class CourierApplicationRepository: ICourierApplicationRepository
    {
        private readonly AppDbContext _context;
        public CourierApplicationRepository(AppDbContext context) { _context = context; }

        public async Task<List<CourierApplication>> GetAllApplicationsAsync() => await _context.Set<CourierApplication>().ToListAsync();

        public async Task<CourierApplication?> GetApplicationByIdAsync(int id) => await _context.Set<CourierApplication>().FirstOrDefaultAsync(user => user.Id == id);

        public async Task<CourierApplication?> GetApplicationByCourierIdAsync(int courierId) => 
            await _context.Set<CourierApplication>().FirstOrDefaultAsync(app => app.CourierId == courierId);

        public async Task CreateApplicationAsync(CourierApplication CourierApplication)
        {
            await _context.Set<CourierApplication>().AddAsync(CourierApplication);
        }

        public Task UpdateApplicationAsync(CourierApplication CourierApplication)
        {
            _context.Set<CourierApplication>().Update(CourierApplication);
            return Task.CompletedTask;
        }

        public Task DeleteApplicationByIdAsync(CourierApplication CourierApplication)
        {
            _context.Set<CourierApplication>().Remove(CourierApplication);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
