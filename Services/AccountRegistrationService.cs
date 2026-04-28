using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class AccountRegistrationService
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly OperatorPlacesService _operatorPlacesService;

        public AccountRegistrationService(IAccountRepository accountsRepo, OperatorPlacesService operatorPlacesService)
        {
            _accountsRepository = accountsRepo;
            _operatorPlacesService = operatorPlacesService;
        }

        private async Task<Account> RegisterAccount(CreateAccountRequest dto, AccountRoles role)
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

            return account;
        }

        public async Task<int> RegisterAdminAsync(CreateAccountRequest dto)
        {
            Account account = await RegisterAccount(dto, AccountRoles.Admin);
            return account.Id;
        }

        public async Task<int> RegisterOperatorAsync(CreateAccountRequest dto, int placeId)
        {
            Account account = await RegisterAccount(dto, AccountRoles.Operator);

            var operatorPlace = await _operatorPlacesService.BuildOperatorPlace(account.Id, placeId);
            await _operatorPlacesService.CreateOperatorPlaceAsync(account.Id, placeId);

            return account.Id;
        }
    }
}
