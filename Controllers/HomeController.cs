using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Delwings.Models;
using Delwings.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace Delwings.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrdersService _ordersService;
        private readonly ApiService _apiService;

        public HomeController(OrdersService ordersService, ApiService apiService) 
        { 
            _ordersService = ordersService;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult BadReq()
        {
            return View();
        }

        public async Task<IActionResult> Calculator(string fromCity, string toCity)
        {
            if (fromCity == null || toCity == null) { return RedirectToAction("BadReq", "Home"); }
            var coordinates = await _apiService.GetPointsAsync(fromCity, toCity);

            if (coordinates == null) { return RedirectToAction("BadReq", "Home"); }

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

            return View(calculatorModel);
        }

        [HttpGet]
        public async Task<IActionResult> TrackOrder(int trackId)
        {
            Order? order = await _ordersService.GetOrderByIdAsync(trackId);

            if (order == null)
            {
                ViewBag.ErrorMessage = "Order not found";
                return View("Index");
            }

            return RedirectToAction("Index", "Orders", new { trackId });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
