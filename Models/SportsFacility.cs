using System.ComponentModel.DataAnnotations;
using PruebaCsharp.Enums;

namespace PruebaCsharp.Models
{
    public class SportsFacility
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [StringLength(30, ErrorMessage = "The name cannot exceed 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The type of space is required.")]
        public TypeOfSpace TypeOfSpace { get; set; }

        [Required(ErrorMessage = "The capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The capacity must be greater than zero.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "The availability is required.")]
        public AvailabilityStatus Availability { get; set; } = AvailabilityStatus.Available;

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}