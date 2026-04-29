using Delwings.Models;
using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Repositories.Interfaces;
using System.Security.Principal;

namespace Delwings.Services
{
    public class AccountRegistrationService
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly OperatorPlacesService _operatorPlacesService;
        private readonly CourierApplicationService _courierApplicationService;

        public AccountRegistrationService(IAccountRepository accountsRepo, OperatorPlacesService operatorPlacesService, CourierApplicationService courierApplicationService)
        {
            _accountsRepository = accountsRepo;
            _operatorPlacesService = operatorPlacesService;
            _courierApplicationService = courierApplicationService;
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

        public async Task<int> RegisterUserAsync(CreateAccountRequest dto)
        {
            Account account = await RegisterAccount(dto, AccountRoles.User);
            return account.Id;
        }

        public async Task<int> RegisterCourierAsync(CreateAccountRequest dto)
        {
            Account account = await RegisterAccount(dto, AccountRoles.Courier);

            await _courierApplicationService.CreateApplicationAsync(
               new CourierApplication
               {
                   CourierId = account.Id,
                   Name = dto.Name,
                   Surname = dto.Surname,
                   Phone = dto.Phone,
                   Status = CourierStatuses.Pending
               }
           );

            return account.Id;
        }
    }
}
