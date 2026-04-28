using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Data;
using PruebaCsharp.Models;

namespace PruebaCsharp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<string?> CreateAsync(User user)
        {
            NormalizeUser(user);

            bool documentExists = await _context.Users
                .AnyAsync(u => u.DocumentId == user.DocumentId);

            if (documentExists)
            {
                return "There is already a user with this document id.";
            }

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == user.Email);

            if (emailExists)
            {
                return "There is already a user with this email.";
            }

            user.TimeRegister = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> UpdateAsync(User user)
        {
            NormalizeUser(user);

            User? existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                return "The user was not found.";
            }

            bool documentExists = await _context.Users
                .AnyAsync(u => u.DocumentId == user.DocumentId && u.Id != user.Id);

            if (documentExists)
            {
                return "There is already another user with this document id.";
            }

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == user.Email && u.Id != user.Id);

            if (emailExists)
            {
                return "There is already another user with this email.";
            }

            existingUser.Name = user.Name;
            existingUser.DocumentId = user.DocumentId;
            existingUser.Phone = user.Phone;
            existingUser.Email = user.Email;

            await _context.SaveChangesAsync();

            return null;
        }

        private void NormalizeUser(User user)
        {
            user.Name = user.Name.Trim();
            user.DocumentId = user.DocumentId.Trim();
            user.Phone = user.Phone.Trim();
            user.Email = user.Email.Trim().ToLower();
        }
    }
}