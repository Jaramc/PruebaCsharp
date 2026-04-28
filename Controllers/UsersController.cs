using Microsoft.AspNetCore.Mvc;
using PruebaCsharp.Models;
using PruebaCsharp.Services;

namespace PruebaCsharp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            List<User> users = await _userService.GetAllAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(int id)
        {
            User? user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            string? errorMessage = await _userService.CreateAsync(user);

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            User? user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            string? errorMessage = await _userService.UpdateAsync(user);

            if (errorMessage != null)
            {
                ModelState.AddModelError("", errorMessage);
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}