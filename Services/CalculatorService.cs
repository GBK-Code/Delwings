using Delwings.Models.Basic;

namespace Delwings.Services
{
    public class CalculatorService
    {
        private readonly ApiService _apiService;

        public CalculatorService(ApiService apiService) { _apiService = apiService; }

        public async Task<Calculator?> BuildModel(string fromCity, string toCity)
        {
            if (fromCity == null || toCity == null) { return null; }
            var coordinates = await _apiService.GetPointsAsync(fromCity, toCity);

            if (coordinates == null) { return null; }

            double fromLat = coordinates["fromLat"];
            double toLat = coordinates["toLat"];
            double fromLon = coordinates["fromLon"];
            double toLon = coordinates["toLon"];

            double deltaLat = fromLat - toLat;
            double deltaLon = fromLon - toLon;

            double dist = Math.Sqrt(Math.Pow(deltaLat, 2) + Math.Pow(deltaLon, 2));

            var calculatorModel = new Calculator
            {
                fromLat = fromLat,
                toLat = toLat,
                fromLon = fromLon,
                toLon = toLon,
                fromCity = fromCity,
                toCity = toCity,
                dist = Math.Round(dist * 111, 2)
            };

            return calculatorModel;
        }
    }
}
