using Delwings.Models.ViewModels;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Controllers
{
    public class UtilityController: Controller
    {
        private readonly QRPageService _qrPageService;

        public UtilityController(QRPageService qrPageService)
        {
            _qrPageService = qrPageService;
        }

        public async Task<IActionResult> QRPage(int orderId)
        {
            QRPageVM? viewModel = await _qrPageService.Build(orderId);
            return View(viewModel);
        }
    }
}
