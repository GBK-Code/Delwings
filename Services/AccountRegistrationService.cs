using Delwings.Models;
using Delwings.Models.DTO;
using Delwings.Models.Enums;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class AccountRegistrationService
    {
        private readonly IAccountRepository _accountsRepository;

        public AccountRegistrationService(IAccountRepository accountsRepo)
        {
            _accountsRepository = accountsRepo;
        }

        public async Task<int> RegisterAccount(CreateAccountDTO dto, AccountRoles role)
        {
            Account account = new Account()
            {
                Login = dto.Login,
                Password = dto.Password,
                Name = dto.Name,
                Surname = dto.Surname,
                Phone = dto.Phone,
                Email = dto.Email,
                Role = role
            };

            await _accountsRepository.CreateAccountAsync(account);
            await _accountsRepository.SaveChangesAsync();

            return account.Id;
        }

        public async Task<int> RegisterAdminAsync(CreateAccountDTO dto)
        {
            int accountId = await RegisterAccount(dto, AccountRoles.Admin);
            return accountId;
        }
    }
}
