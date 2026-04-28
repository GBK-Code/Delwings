using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Delwings.Models.Requests;


namespace Delwings.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Cookies")]
    public class AdminController : Controller
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;
        private readonly AdminDashboardService _adminDashboardService;
        private readonly AccountRegistrationService _accountRegistrationService;

        public AdminController 
            (
            AccountService accountService, 
            CourierApplicationService courierApplicationService, 
            AdminDashboardService adminDashboardService,
            AccountRegistrationService accountRegistrationService
            )
        { 
            _accountService = accountService;
            _courierApplicationService = courierApplicationService;
            _adminDashboardService = adminDashboardService;
            _accountRegistrationService = accountRegistrationService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string tab)
        {
            var userName = User.Identity!.Name!;
            var vm = await _adminDashboardService.Build(tab, userName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptApplication(int applicationId)
        {
            var success = await _courierApplicationService.SetStatusAsync(applicationId, CourierStatuses.Accepted);
            if (!success) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Admin", new { tab = "couriers" });
        }

        [HttpPost]
        public async Task<IActionResult> DeclineApplication(int applicationId)
        {
            var success = await _courierApplicationService.SetStatusAsync(applicationId, CourierStatuses.Declined);
            if (!success) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Admin", new { tab = "couriers" });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOperator(CreateAccountRequest request, int placeId)
        {
            if (request.Password != request.PasswordConfirm) { return RedirectToAction("BadReq", "Home"); }

            int operatorId = await _accountRegistrationService.RegisterOperatorAsync(request, placeId);

            return RedirectToAction("Dashboard", "Admin", new { tab = "operators" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOperatorById(int id)
        {
            var success = await _accountService.DeleteOperatorAccountByOperatorIdAsync(id);
            if (!success) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Admin", new { tab = "operators" });
        }

        [HttpPost]
        public async Task<IActionResult> AssignOperatorToEmptyPlace(int operatorId, int placeId)
        {
            var success = await _accountService.AssignOperatorToEmptyPlace(operatorId, placeId);
            if (!success) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Admin", new { tab = "operators" });
        }
    }

}
