using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PruebaCsharp.ViewModels
{
    public class ReservationCreateViewModel
    {
        [Required(ErrorMessage = "Please select a valid user.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a valid sports facility.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sports facility.")]
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