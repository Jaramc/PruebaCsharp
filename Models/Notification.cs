using System.ComponentModel.DataAnnotations;

namespace PruebaCsharp.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The reservation is required.")]
        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; } = null!;

        [Required(ErrorMessage = "The email recipient is required.")]
        [StringLength(80, ErrorMessage = "The email recipient cannot exceed 80 characters.")]
        [EmailAddress(ErrorMessage = "The email format is not valid.")]
        public string EmailRecipient { get; set; } = string.Empty;

        [Required(ErrorMessage = "The subject is required.")]
        [StringLength(50, ErrorMessage = "The subject cannot exceed 50 characters.")]
        public string Subject { get; set; } = string.Empty;

        public DateTime DateSent { get; set; } = DateTime.Now;

        public bool WasSent { get; set; } = false;

        [StringLength(255, ErrorMessage = "The error message cannot exceed 255 characters.")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}