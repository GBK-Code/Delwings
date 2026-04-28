using Delwings.Models;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOperatorPlacesRepository _operatorPlacesRepository;

        public AccountService(IAccountRepository accountRepository, IOperatorPlacesRepository operatorPlacesRepository)
        {
            _accountRepository = accountRepository;
            _operatorPlacesRepository = operatorPlacesRepository;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<Account?> GetAccountByLoginAsync(string login)
        {
            return await _accountRepository.GetAccountByLoginAsync(login);
        }

        public async Task<int> CreateAccountAsync(Account account)
        {
            await _accountRepository.CreateAccountAsync(account);
            await _accountRepository.SaveChangesAsync();
            return account.Id;
        }

        public async Task<bool> UpdateAccountAsync(int id, Account newData)
        {
            var existingAccount = await _accountRepository.GetAccountByIdAsync(id);
            if (existingAccount == null) { return false; }

            existingAccount.Name = newData.Name;
            existingAccount.Surname = newData.Surname;
            existingAccount.Login = newData.Login;
            existingAccount.Phone = newData.Phone;
            existingAccount.Email = newData.Email;
            existingAccount.Role = newData.Role;
            existingAccount.Password = newData.Password;

            await _accountRepository.UpdateAccountAsync(existingAccount);
            await _accountRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var accountToDelete = await _accountRepository.GetAccountByIdAsync(id);
            if (accountToDelete == null) { return false; }

            await _accountRepository.DeleteAccountAsync(accountToDelete);
            await _accountRepository.SaveChangesAsync();
            return true;
        }

        // Particular case
        public async Task<bool> DeleteAdminAccountByIdAsync(int id)
        {
            var accountToDelete = await _accountRepository.GetAccountByIdAsync(id);

            if (accountToDelete == null) { return false; }
            if (accountToDelete.Role != Models.Enums.AccountRoles.Admin) { return false; }

            return await DeleteAccountByIdAsync(accountToDelete.Id);
        }

        public async Task<bool> DeleteOperatorAccountByOperatorIdAsync(int id)
        {
            var placeToDelete = await _operatorPlacesRepository.GetOperatorPlaceByOperatorIdAsync(id);
            var accountToDelete = await _accountRepository.GetAccountByIdAsync(id);
            if (placeToDelete == null || accountToDelete == null) { return false; }

            await _accountRepository.DeleteAccountAsync(accountToDelete);
            await _accountRepository.SaveChangesAsync();

            await _operatorPlacesRepository.DeleteOperatorPlace(placeToDelete);
            await _operatorPlacesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignOperatorToEmptyPlace(int operatorId, int placeId)
        {
            var operPlace = await _operatorPlacesRepository.GetOperatorPlaceByOperatorIdAsync(operatorId);
            if (operPlace == null) { return false; }

            operPlace.PlaceId = placeId;

            await _operatorPlacesRepository.UpdateOperatorPlace(operPlace);
            await _operatorPlacesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckValidity(Account? account, string password)
        {
            if (account == null ) { return false; }
            return account.Password == password;
        }
    }
}
