using Delwings.Context;
using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delwings.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        public readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) {  _context = context; }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Set<Account>().ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Set<Account>().FirstOrDefaultAsync(account => account.Id == id);
        }

        public async Task<Account?> GetAccountByLoginAsync(string login)
        {
            return await _context.Set<Account>().FirstOrDefaultAsync(account => account.Login == login);
        }

        public async Task CreateAccountAsync(Account account)
        {
            var dbset = _context.Set<Account>();
            await dbset.AddAsync(account);
        }

        public Task UpdateAccountAsync(Account account)
        {
            _context.Set<Account>().Update(account);
            return Task.CompletedTask;
        }

        public Task DeleteAccountAsync(Account account)
        {
            _context.Set<Account>().Remove(account);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
