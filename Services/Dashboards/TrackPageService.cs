using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Models.Results;
using System.Security.Claims;

namespace Delwings.Services.Dashboards
{
    public class TrackPageService
    {
        private readonly OrdersService _ordersService;
        private readonly PlaceService _placeService;
        private readonly OrdersHistoryService _ordersHistoryService;
        private readonly AccountService _accountService;
        private readonly OperatorPlacesService _operatorPlacesService;

        public TrackPageService
            (
                OrdersService ordersService,
                PlaceService placeService,
                OrdersHistoryService ordersHistory,
                AccountService accountService,
                OperatorPlacesService operatorPlacesService
            )
        {
            _ordersService = ordersService;
            _placeService = placeService;
            _ordersHistoryService = ordersHistory;
            _accountService = accountService;
            _operatorPlacesService = operatorPlacesService;
        }

        private async Task<List<Place>> GetVisitedPlaces(List<OrdersHistory> history)
        {
            List<Place> visited = new List<Place>();

            foreach (var record in history)
            {
                var placeToAdd = await _placeService.GetPlaceByIdAsync(record.PlaceId);
                if (placeToAdd != null) { visited.Add(placeToAdd); }
            }
            
            return visited;
        }


        private async Task<OperatorTrackResult?> GetOperatorTrackData(ClaimsPrincipal user)
        {
            string? identityName = user.Identity?.Name;
            if (identityName == null) { return null; }

            var oper = await _accountService.GetAccountByLoginAsync(identityName);
            if (oper == null) { return null; }

            Account operatorAccount = oper;
            Place? placeToAssign = null;

            // Assign place
            var operatorPlace = await _operatorPlacesService.GetOperatorPlaceByOperatorIdAsync(operatorAccount.Id);
            if (operatorPlace == null) { return null; }

            var placeId = operatorPlace.PlaceId;
            placeToAssign = await _placeService.GetPlaceByIdAsync(placeId);
            if (placeToAssign == null) { return null; }

            OperatorTrackResult result = new OperatorTrackResult()
            {
                Operator = operatorAccount,
                Place = placeToAssign
            };

            return result;
        }

        public async Task<TrackerPageVM?> Build(string trackId, ClaimsPrincipal user)
        {
            var order = await _ordersService.GetOrderByTrackIdAsync(trackId);
            if (order == null) { return null; }

            var place = await _placeService.GetPlaceByIdAsync(order.CurrentLocationId);
            if (place == null) { return null; } 

            var history = await _ordersHistoryService.GetAllHistoryAsync();
            history = history.Where(rec => rec.OrderId == order.Id).ToList();

            List<Place> visitedPlaces = await GetVisitedPlaces(history);

            OperatorTrackResult? operatorResult = new OperatorTrackResult();

            bool userIsOperator = user.IsInRole(AccountRoles.Operator.ToString());
            bool userIsCourier = user.IsInRole(AccountRoles.Courier.ToString());

            if (userIsOperator)
            {
                operatorResult = await GetOperatorTrackData(user);
                if (operatorResult == null) { return null; }
            }

            Account? courier = null;

            if (userIsCourier)
            {
                string? identityName = user.Identity?.Name;
                if (identityName == null) { return null; }

                courier = await _accountService.GetAccountByLoginAsync(identityName);
                if (courier == null) { return null; }
            }


            var trackVM = new TrackerPageVM
            {
                Order = order,
                Current = place,
                IsOperator = userIsOperator,
                IsCourier = userIsCourier,
                Operator = operatorResult.Operator,
                OperatorPlace = operatorResult.Place,
                Courier = courier,
                OrderPlaces = visitedPlaces
            };

            return trackVM;
        }
    }
}
