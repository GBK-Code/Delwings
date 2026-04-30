using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class DataGeneratorService
    {
        private readonly OrderTokensGenerator _tokensGenerator;
        private readonly PlaceService _placeService;
        private readonly AccountService _accountService;
        private readonly IOrdersRepository _ordersRepo;
        private readonly IPlaceRepository _placeRepository;

        public DataGeneratorService
            (
                OrderTokensGenerator tokensGenerator,
                PlaceService placeService, 
                AccountService accountService,
                IOrdersRepository ordersRepo,
                IPlaceRepository placeRepo
            )
        {
            _tokensGenerator = tokensGenerator;
            _placeService = placeService;
            _accountService = accountService;
            _ordersRepo = ordersRepo;
            _placeRepository = placeRepo;
        }

        private DateTime GenerateRandomDateTime(Random rng, string from, string to)
        {
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);

            TimeSpan range = toDate - fromDate;

            long randomTicks = (long)(rng.NextDouble() * range.Ticks);

            return fromDate.AddTicks(randomTicks);
        }

        private string GenerateRandomAddress(Random rng)
        {
            List<string> cities = new List<string> { "Astana", "Almaty", "Semey", "Temirtau", "Karaganda", "Aktobe" };
            List<string> streets = new List<string> { "Abai", "Kenesary", "Mira", "Satpaev", "Abylai" };
            int house = rng.Next(1, 80);

            return $"{cities[rng.Next(cities.Count)]}, {streets[rng.Next(streets.Count)]}, {house}";
        }

        private string GetRandomInsuranceCompany(Random rng)
        {
            List<string> companies = new List<string> { "Eurasia", "Nomad", "Freedom", "Centras" };
            return companies[rng.Next(companies.Count)];
        }

        private async Task<int> GetRandomCourierId(Random rng, List<Account> couriers)
        {
            Account courier = couriers[rng.Next(couriers.Count)];

            return courier.Id;
        }

        private async Task<Order> CreateRandomOrder(Random rng, List<OrderTypes> types, List<Account> couriers, int numberOfPlaces)
        {
            DateTime randomDT = GenerateRandomDateTime(rng, "2026-01-01", "2026-12-31");

            OrderTypes orderType = types[rng.Next(types.Count)];
            string date = randomDT.Date.ToString();
            string time = TimeOnly.FromDateTime(randomDT).ToString();
            string trackId = _tokensGenerator.GenerateTrackId();
            int receiverNumber = _tokensGenerator.GenerateReceiverNumber();
            int senderId = rng.Next(1, 100);
            bool isCarried = (rng.Next(2) == 1) ? true : false;
            string? receiverContact = "contact@gmail.com";
            string? destination = GenerateRandomAddress(rng);
            int currentLocationId = rng.Next(1, numberOfPlaces);
            string? insuranceCompany = null;
            int? insurancePrice = null;
            int? courierId = null;

            if (orderType == OrderTypes.Insured)
            {
                insuranceCompany = GetRandomInsuranceCompany(rng);
                insurancePrice = rng.Next(10000, 50000);
            }

            if (orderType == OrderTypes.Express)
            {
                courierId = await GetRandomCourierId(rng, couriers);
            }

            Order order = new Order
            {
                OrderType = orderType,
                Date = DateOnly.FromDateTime(randomDT).ToString(),
                Time = time,
                TrackId = trackId,
                ReceiverNumber = receiverNumber,
                SenderId = senderId,
                IsCarried = isCarried,
                ReceiverContact = receiverContact,
                Destination = destination,
                CurrentLocationId = currentLocationId,
                CourierId = courierId,
                InsuranceCompany = insuranceCompany,
                InsurancePrice = insurancePrice
            };

            return order;
        }

        private Place CreateRandomPlace(Random rng, List<string> countires, List<PlaceTypes> types, string address)
        {
            string[] splittedAddress = address.Split(",");

            string country = countires[rng.Next(countires.Count)];
            string city = splittedAddress[0].Replace(" ", "");
            string street = splittedAddress[1].Replace(" ", "");
            string houseNumber = splittedAddress[2].Replace(" ", "");

            string resultAddress = $"{street}, {houseNumber}";

            Place place = new Place
            {
                Country = country,
                City = city,
                Address = resultAddress,
                Contact = "contact@gmail.com",
                Type = types[rng.Next(types.Count)]
            };

            return place;
        }

        public async Task GenerateOrders(int amount)
        {
            Random rng = new Random();

            List<OrderTypes> types = new List<OrderTypes> { OrderTypes.Ordinary, OrderTypes.Insured, OrderTypes.Express };
            List<Place> places = await _placeService.GetAllPlacesAsync();
            int numberOfPlaces = places.Count;

            List<Account> couriers = await _accountService.GetAllAccountsAsync();
            couriers = couriers.Where(acc => acc.Role == AccountRoles.Courier).ToList();


            for (int i=0; i < amount;i++)
            {
                Order order = await CreateRandomOrder(rng, types, couriers, numberOfPlaces);
                await _ordersRepo.AddOrderAsync(order);
            }
            await _ordersRepo.SaveChangesAsync();
        }

        public async Task GeneratePlaces(int amount)
        {
            Random rng = new Random();

            List<string> countries = new List<string> { "Kazakhstan" };
            List<string> cities = new List<string> { "Astana", "Almaty", "Semey", "Temirtau", "Karaganda", "Aktobe" };
            List<PlaceTypes> placeTypes = new List<PlaceTypes> { PlaceTypes.AcceptPoint, PlaceTypes.SortingPoint, PlaceTypes.PickUpPoint };

            for (int i=0; i < amount;i++)
            {
                string randomAddress = GenerateRandomAddress(rng);
                Place place = CreateRandomPlace(rng, countries, placeTypes, randomAddress);

                await _placeRepository.CreatePlaceAsync(place);
            }
            await _placeRepository.SaveChangesAsync();
        }
    }
}
