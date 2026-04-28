using Delwings.Models;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository) { _repository = repository; }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _repository.GetAllAccountsAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _repository.GetAccountByIdAsync(id);
        }

        public async Task<Account?> GetAccountByLoginAsync(string login)
        {
            return await _repository.GetAccountByLoginAsync(login);
        }

        public async Task<int> CreateAccountAsync(Account account)
        {
            await _repository.CreateAccountAsync(account);
            await _repository.SaveChangesAsync();
            return account.Id;
        }

        public async Task<bool> UpdateAccountAsync(int id, Account newData)
        {
            var existingAccount = await _repository.GetAccountByIdAsync(id);
            if (existingAccount == null) { return false; }

            existingAccount.Name = newData.Name;
            existingAccount.Surname = newData.Surname;
            existingAccount.Login = newData.Login;
            existingAccount.Phone = newData.Phone;
            existingAccount.Email = newData.Email;
            existingAccount.Role = newData.Role;
            existingAccount.Password = newData.Password;

            await _repository.UpdateAccountAsync(existingAccount);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var accountToDelete = await _repository.GetAccountByIdAsync(id);
            if (accountToDelete == null) { return false; }

            await _repository.DeleteAccountAsync(accountToDelete);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckValidity(Account? account, string password)
        {
            if (account == null ) { return false; }
            return account.Password == password;
        }
    }
}
