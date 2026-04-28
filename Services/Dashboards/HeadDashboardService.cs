using Delwings.Models;
using Delwings.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Delwings.Services.Dashboards
{
    public class HeadDashboardService
    {
        private readonly AccountService _accountService;
        private readonly PlaceService _placeService;

        public HeadDashboardService(AccountService accountService, PlaceService placeService)
        {
            _accountService = accountService;
            _placeService = placeService;
        }

        public async Task<HeadPageVM?> BuildAsync(string tab, string identityName)
        {
            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return null; }

            var users = await _accountService.GetAllAccountsAsync();
            List<Account> _admins = users.Where(user => user.Role == AccountRoles.Admin).ToList();

            var places = await _placeService.GetAllPlacesAsync();

            var pageVM = new HeadPageVM
            {
                Me = user,
                Tab = tab,
                AdminAccounts = _admins,
                Places = places
            };

            return pageVM;
        }
    }
}
