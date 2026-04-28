using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PruebaCsharp.ViewModels
{
    public class ReservationCreateViewModel
    {
        [Required(ErrorMessage = "The user is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The sports facility is required.")]
        public int SportsFacilityId { get; set; }

        [Required(ErrorMessage = "The reservation date is required.")]
        public DateTime ReservationDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "The start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "The end time is required.")]
        public TimeSpan EndTime { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> SportsFacilities { get; set; } = new List<SelectListItem>();
    }
}