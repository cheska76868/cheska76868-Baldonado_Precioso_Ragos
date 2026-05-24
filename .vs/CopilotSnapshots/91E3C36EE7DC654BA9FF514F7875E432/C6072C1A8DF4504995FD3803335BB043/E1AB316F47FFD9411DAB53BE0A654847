using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BarangayTelemedicine.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "Patient"; // Admin, HealthWorker, Patient

        // New: CreatedAt must be populated on insert to match DB non-nullable column
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}