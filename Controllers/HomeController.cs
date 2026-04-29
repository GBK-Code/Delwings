using Microsoft.AspNetCore.Mvc;
using Delwings.Models;
using Delwings.Services;
using System.Diagnostics;


namespace Delwings.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrdersService _ordersService;
        private readonly ApiService _apiService;
        private readonly CalculatorService _calculatorService;

        public HomeController(OrdersService ordersService, ApiService apiService, CalculatorService calculatorService) 
        { 
            _ordersService = ordersService;
            _apiService = apiService;
            _calculatorService = calculatorService;
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

        public async Task<IActionResult> CalculatorPage(string fromCity, string toCity)
        {
            Calculator? calculatorModel = await _calculatorService.BuildModel(fromCity, toCity);
            if (calculatorModel == null) { return BadReq(); }

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
