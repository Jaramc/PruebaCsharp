using System.ComponentModel.DataAnnotations;

namespace PruebaCsharp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [StringLength(60, ErrorMessage = "The name cannot exceed 60 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The document id is required.")]
        [StringLength(30, ErrorMessage = "The document id cannot exceed 30 characters.")]
        public string DocumentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The phone is required.")]
        [StringLength(15, ErrorMessage = "The phone cannot exceed 15 characters.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required.")]
        [StringLength(80, ErrorMessage = "The email cannot exceed 80 characters.")]
        [EmailAddress(ErrorMessage = "The email format is not valid.")]
        public string Email { get; set; } = string.Empty;

        public DateTime TimeRegister { get; set; } = DateTime.Now;

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}