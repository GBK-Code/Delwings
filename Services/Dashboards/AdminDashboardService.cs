using Delwings.Models;
using Delwings.Models.Enums;

namespace Delwings.Services.Dashboards
{
    public class AdminDashboardService
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;
        private readonly PlaceService _placesService;
        private readonly OperatorPlacesService _operatorPlacesService;

        public AdminDashboardService(
            AccountService accountService,
            CourierApplicationService courierApplicationService,
            PlaceService placesService,
            OperatorPlacesService operatorPlacesService
            )
        {
            _accountService = accountService;
            _courierApplicationService = courierApplicationService;
            _placesService = placesService;
            _operatorPlacesService = operatorPlacesService;
        }

        public async Task<AdminPageVM> Build(string tab, string identityName)
        {
            var me = await _accountService.GetAccountByLoginAsync(identityName);
            var operatorsList = await _accountService.GetAllAccountsAsync();
            var applicationsList = await _courierApplicationService.GetAllApplicationsAsync();

            var places = await _placesService.GetAllPlacesAsync();
            var operatorPlaces = await _operatorPlacesService.GetAllOperatorPlacesAsync();

            var vm = new AdminPageVM
            {
                Me = me,
                Tab = tab,
                OperatorsList = operatorsList.Where(user => user.Role == AccountRoles.Operator).ToList(),
                PlacesList = places.ToList(),
                CouriersApplicationsList = applicationsList.Where(app => app.Status == CourierStatuses.Pending).ToList(),
                OperatorPlacesList = operatorPlaces.ToList()
            };

            return vm;
        }
    }
}
