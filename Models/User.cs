using System.ComponentModel.DataAnnotations;

namespace PruebaCsharp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "The name must be between 2 and 60 characters.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "The name can only contain letters and spaces.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The document id is required.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "The document id must be between 6 and 30 characters.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "The document id can only contain numbers.")]
        public string DocumentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The phone is required.")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "The phone must be between 7 and 15 characters.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "The phone can only contain numbers.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required.")]
        [StringLength(80, ErrorMessage = "The email cannot exceed 80 characters.")]
        [EmailAddress(ErrorMessage = "The email format is not valid.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email must have a valid format, for example user@example.com.")]
        public string Email { get; set; } = string.Empty;

        public DateTime TimeRegister { get; set; } = DateTime.Now;

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}