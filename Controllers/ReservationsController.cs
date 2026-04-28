using Microsoft.AspNetCore.Mvc;
using PruebaCsharp.Models;
using PruebaCsharp.Services;
using PruebaCsharp.ViewModels;

namespace PruebaCsharp.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            List<Reservation> reservations = await _reservationService.GetAllAsync();
            return View(reservations);
        }

        public async Task<IActionResult> Details(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public async Task<IActionResult> Create()
        {
            ReservationCreateViewModel model = await _reservationService.BuildCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _reservationService.LoadSelectListsAsync(model);
                return View(model);
            }

            string? errorMessage = await _reservationService.CreateReservationAsync(model);

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
                await _reservationService.LoadSelectListsAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            string? errorMessage = await _reservationService.CancelAsync(id);

            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = errorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Reservation canceled successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            string? errorMessage = await _reservationService.CompleteAsync(id);

            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = errorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Reservation completed successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ByUser(int userId)
        {
            List<Reservation> reservations = await _reservationService.GetByUserAsync(userId);
            return View("Index", reservations);
        }

        public async Task<IActionResult> BySportsFacility(int sportsFacilityId)
        {
            List<Reservation> reservations = await _reservationService.GetBySportsFacilityAsync(sportsFacilityId);
            return View("Index", reservations);
        }
    }
}