using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Data;
using PruebaCsharp.Enums;
using PruebaCsharp.Models;
using PruebaCsharp.ViewModels;

namespace PruebaCsharp.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ReservationService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.SportsFacility)
                .OrderByDescending(r => r.ReservationDate)
                .ThenBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.SportsFacility)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReservationCreateViewModel> BuildCreateViewModelAsync()
        {
            ReservationCreateViewModel model = new ReservationCreateViewModel();

            await LoadSelectListsAsync(model);

            return model;
        }

        public async Task LoadSelectListsAsync(ReservationCreateViewModel model)
        {
            model.Users = await _context.Users
                .OrderBy(u => u.Name)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name + " - " + u.DocumentId
                })
                .ToListAsync();

            model.SportsFacilities = await _context.SportsFacilities
                .Where(s => s.Availability == AvailabilityStatus.Available)
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name + " - " + s.TypeOfSpace + " - Capacity: " + s.Capacity
                })
                .ToListAsync();
        }

        public async Task<string?> CreateReservationAsync(ReservationCreateViewModel model)
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == model.UserId);

            if (user == null)
            {
                return "The selected user does not exist.";
            }

            SportsFacility? sportsFacility = await _context.SportsFacilities
                .FirstOrDefaultAsync(s => s.Id == model.SportsFacilityId);

            if (sportsFacility == null)
            {
                return "The selected sports facility does not exist.";
            }

            if (sportsFacility.Availability != AvailabilityStatus.Available)
            {
                return "The selected sports facility is not available.";
            }

            if (model.ReservationDate.Date < DateTime.Today)
            {
                return "Reservations cannot be created for past dates.";
            }

            if (model.EndTime <= model.StartTime)
            {
                return "The end time must be greater than the start time.";
            }

            if (model.ReservationDate.Date == DateTime.Today && model.StartTime <= DateTime.Now.TimeOfDay)
            {
                return "Reservations cannot be created for a time that has already passed.";
            }

            bool sportsFacilityConflict = await _context.Reservations
                .AnyAsync(r =>
                    r.SportsFacilityId == model.SportsFacilityId &&
                    r.ReservationDate.Date == model.ReservationDate.Date &&
                    r.Status == ReservationStatus.Active &&
                    model.StartTime < r.EndTime &&
                    model.EndTime > r.StartTime);

            if (sportsFacilityConflict)
            {
                return "The sports facility already has an active reservation in that time range.";
            }

            bool userConflict = await _context.Reservations
                .AnyAsync(r =>
                    r.UserId == model.UserId &&
                    r.ReservationDate.Date == model.ReservationDate.Date &&
                    r.Status == ReservationStatus.Active &&
                    model.StartTime < r.EndTime &&
                    model.EndTime > r.StartTime);

            if (userConflict)
            {
                return "The user already has an active reservation in that time range.";
            }

            Reservation reservation = new Reservation
            {
                UserId = model.UserId,
                SportsFacilityId = model.SportsFacilityId,
                ReservationDate = model.ReservationDate.Date,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.Now
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            await _emailService.SendReservationConfirmationAsync(reservation.Id);

            return null;
        }

        public async Task<string?> CancelAsync(int id)
        {
            Reservation? reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return "The reservation was not found.";
            }

            if (reservation.Status != ReservationStatus.Active)
            {
                return "Only active reservations can be canceled.";
            }

            reservation.Status = ReservationStatus.Canceled;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> CompleteAsync(int id)
        {
            Reservation? reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return "The reservation was not found.";
            }

            if (reservation.Status != ReservationStatus.Active)
            {
                return "Only active reservations can be completed.";
            }

            reservation.Status = ReservationStatus.Completed;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<Reservation>> GetByUserAsync(int userId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.SportsFacility)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ThenBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetBySportsFacilityAsync(int sportsFacilityId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.SportsFacility)
                .Where(r => r.SportsFacilityId == sportsFacilityId)
                .OrderByDescending(r => r.ReservationDate)
                .ThenBy(r => r.StartTime)
                .ToListAsync();
        }
    }
}