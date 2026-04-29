using Microsoft.EntityFrameworkCore;
using Delwings.Models.Basic;

namespace Delwings.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Place> Places => Set<Place>();
        public DbSet<CourierApplication> CourierApplications => Set<CourierApplication>();
        public DbSet<OperatorPlace> OperatorPlaces => Set<OperatorPlace>();
        public DbSet<OrdersHistory> OrdersHistory => Set<OrdersHistory>();
        public DbSet<CourierOrder> CourierOrders => Set<CourierOrder>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
