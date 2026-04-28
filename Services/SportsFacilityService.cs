using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Data;
using PruebaCsharp.Enums;
using PruebaCsharp.Models;

namespace PruebaCsharp.Services
{
    public class SportsFacilityService
    {
        private readonly ApplicationDbContext _context;

        public SportsFacilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SportsFacility>> GetAllAsync()
        {
            return await _context.SportsFacilities
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<SportsFacility>> GetByTypeAsync(TypeOfSpace? typeOfSpace)
        {
            if (typeOfSpace == null)
            {
                return await GetAllAsync();
            }

            return await _context.SportsFacilities
                .Where(s => s.TypeOfSpace == typeOfSpace)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<SportsFacility?> GetByIdAsync(int id)
        {
            return await _context.SportsFacilities
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<string?> CreateAsync(SportsFacility sportsFacility)
        {
            NormalizeSportsFacility(sportsFacility);

            string? validationError = ValidateSportsFacility(sportsFacility);

            if (validationError != null)
            {
                return validationError;
            }

            bool facilityExists = await _context.SportsFacilities
                .AnyAsync(s =>
                    s.Name == sportsFacility.Name &&
                    s.TypeOfSpace == sportsFacility.TypeOfSpace);

            if (facilityExists)
            {
                return "There is already a sports facility with this name and type of space.";
            }

            _context.SportsFacilities.Add(sportsFacility);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> UpdateAsync(SportsFacility sportsFacility)
        {
            NormalizeSportsFacility(sportsFacility);

            string? validationError = ValidateSportsFacility(sportsFacility);

            if (validationError != null)
            {
                return validationError;
            }

            SportsFacility? existingFacility = await _context.SportsFacilities
                .FirstOrDefaultAsync(s => s.Id == sportsFacility.Id);

            if (existingFacility == null)
            {
                return "The sports facility was not found.";
            }

            bool facilityExists = await _context.SportsFacilities
                .AnyAsync(s =>
                    s.Name == sportsFacility.Name &&
                    s.TypeOfSpace == sportsFacility.TypeOfSpace &&
                    s.Id != sportsFacility.Id);

            if (facilityExists)
            {
                return "There is already another sports facility with this name and type of space.";
            }

            existingFacility.Name = sportsFacility.Name;
            existingFacility.TypeOfSpace = sportsFacility.TypeOfSpace;
            existingFacility.Capacity = sportsFacility.Capacity;
            existingFacility.Availability = sportsFacility.Availability;

            await _context.SaveChangesAsync();

            return null;
        }

        private void NormalizeSportsFacility(SportsFacility sportsFacility)
        {
            sportsFacility.Name = sportsFacility.Name.Trim();
        }

        private string? ValidateSportsFacility(SportsFacility sportsFacility)
        {
            if (string.IsNullOrWhiteSpace(sportsFacility.Name))
            {
                return "The name is required.";
            }

            if (sportsFacility.Capacity <= 0)
            {
                return "The capacity must be greater than zero.";
            }

            return null;
        }
    }
}