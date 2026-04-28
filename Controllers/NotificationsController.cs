using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Data;
using PruebaCsharp.Models;

namespace PruebaCsharp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Notification> notifications = await _context.Notifications
                .Include(n => n.Reservation)
                .ThenInclude(r => r.User)
                .Include(n => n.Reservation)
                .ThenInclude(r => r.SportsFacility)
                .OrderByDescending(n => n.DateSent)
                .ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> Details(int id)
        {
            Notification? notification = await _context.Notifications
                .Include(n => n.Reservation)
                .ThenInclude(r => r.User)
                .Include(n => n.Reservation)
                .ThenInclude(r => r.SportsFacility)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }
    }
}