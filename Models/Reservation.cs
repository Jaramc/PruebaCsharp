using System.ComponentModel.DataAnnotations;
using PruebaCsharp.Enums;

namespace PruebaCsharp.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The user is required.")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required(ErrorMessage = "The sports facility is required.")]
        public int SportsFacilityId { get; set; }

        public SportsFacility SportsFacility { get; set; } = null!;

        [Required(ErrorMessage = "The reservation date is required.")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "The start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "The end time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "The status is required.")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}