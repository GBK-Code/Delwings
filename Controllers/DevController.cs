using Delwings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Controllers
{
    [Authorize(Roles = "Head", AuthenticationSchemes = "Cookies")]
    public class DevController : Controller
    {
        private readonly DataGeneratorService _dataGeneratorService;
        private readonly DataEraserService _dataEraserService;

        public DevController(DataGeneratorService dataGeneratorService, DataEraserService dataEraserService)
        {
            _dataGeneratorService = dataGeneratorService;
            _dataEraserService = dataEraserService;
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlaces()
        {
            await _dataGeneratorService.GeneratePlaces(50);
            return RedirectToAction("Dashboard", "Head", new { tab = "devtools" });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOrders()
        {
            await _dataGeneratorService.GenerateOrders(200);
            return RedirectToAction("Dashboard", "Head", new { tab = "devtools" });
        }

        [HttpPost]
        public async Task<IActionResult> ClearOrders()
        {
            await _dataEraserService.EraseOrders();
            return RedirectToAction("Dashboard", "Head", new { tab = "devtools" });
        }

        [HttpPost]
        public async Task<IActionResult> ClearPlaces()
        {
            await _dataEraserService.ErasePlaces();
            return RedirectToAction("Dashboard", "Head", new { tab = "devtools" });
        }
    }
}
