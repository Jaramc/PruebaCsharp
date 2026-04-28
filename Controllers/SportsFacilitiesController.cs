using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PruebaCsharp.Enums;
using PruebaCsharp.Models;
using PruebaCsharp.Services;

namespace PruebaCsharp.Controllers
{
    public class SportsFacilitiesController : Controller
    {
        private readonly SportsFacilityService _sportsFacilityService;

        public SportsFacilitiesController(SportsFacilityService sportsFacilityService)
        {
            _sportsFacilityService = sportsFacilityService;
        }

        public async Task<IActionResult> Index(TypeOfSpace? typeOfSpace)
        {
            List<SportsFacility> sportsFacilities = await _sportsFacilityService.GetByTypeAsync(typeOfSpace);

            ViewBag.TypeOfSpace = typeOfSpace;
            ViewBag.TypeOfSpaces = GetTypeOfSpaceList();

            return View(sportsFacilities);
        }

        public async Task<IActionResult> Details(int id)
        {
            SportsFacility? sportsFacility = await _sportsFacilityService.GetByIdAsync(id);

            if (sportsFacility == null)
            {
                return NotFound();
            }

            return View(sportsFacility);
        }

        public IActionResult Create()
        {
            LoadSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportsFacility sportsFacility)
        {
            if (!ModelState.IsValid)
            {
                LoadSelectLists();
                return View(sportsFacility);
            }

            string? errorMessage = await _sportsFacilityService.CreateAsync(sportsFacility);

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
                LoadSelectLists();
                return View(sportsFacility);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            SportsFacility? sportsFacility = await _sportsFacilityService.GetByIdAsync(id);

            if (sportsFacility == null)
            {
                return NotFound();
            }

            LoadSelectLists();
            return View(sportsFacility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SportsFacility sportsFacility)
        {
            if (id != sportsFacility.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                LoadSelectLists();
                return View(sportsFacility);
            }

            string? errorMessage = await _sportsFacilityService.UpdateAsync(sportsFacility);

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
                LoadSelectLists();
                return View(sportsFacility);
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadSelectLists()
        {
            ViewBag.TypeOfSpaces = GetTypeOfSpaceList();
            ViewBag.AvailabilityStatuses = GetAvailabilityStatusList();
        }

        private SelectList GetTypeOfSpaceList()
        {
            return new SelectList(Enum.GetValues(typeof(TypeOfSpace)));
        }

        private SelectList GetAvailabilityStatusList()
        {
            return new SelectList(Enum.GetValues(typeof(AvailabilityStatus)));
        }
    }
}