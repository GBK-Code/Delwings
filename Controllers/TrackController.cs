using Microsoft.AspNetCore.Mvc;
using Delwings.Services;
using Microsoft.AspNetCore.Authorization;
using Delwings.Services.Dashboards;
using Delwings.Models.ViewModels;

namespace Delwings.Controllers
{
    public class TrackController: Controller
    {
        private readonly TrackPageService _trackPageService;
        private readonly DeliveryService _deliveryService;

        public TrackController (TrackPageService trackPageService, DeliveryService deliveryService)
        { 
            _trackPageService = trackPageService;
            _deliveryService = deliveryService;
        }

        public async Task<IActionResult> Index(string trackToken)
        {
            TrackerPageVM? viewModel = await _trackPageService.Build(trackToken, User);
            if (viewModel == null) { return RedirectToAction("BadReq", "Home"); }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> AssignPlace(int orderId, int placeId, string trackId)
        {
            bool success = await _deliveryService.OperatorConfirm(orderId, placeId, trackId);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Index", "Track", new { trackToken = trackId });
        }

        [HttpPost]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> CourierConfirm(int orderId, int courierId, string trackId)
        {
            bool success = await _deliveryService.CourierConfirm(orderId, courierId);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Index", "Track", new {trackToken = trackId});
        }
    }
}
