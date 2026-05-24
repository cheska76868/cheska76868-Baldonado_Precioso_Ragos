using System.ComponentModel.DataAnnotations;

namespace BarangayTelemedicine.Models
{
	public class HealthWorker
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "First name is required.")]
		[StringLength(50)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name is required.")]
		[StringLength(50)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Position is required.")]
		[StringLength(100)]
		public string Position { get; set; } = string.Empty; // e.g., Barangay Health Worker, Nurse, Doctor

		[Required(ErrorMessage = "Barangay assigned is required.")]
		[StringLength(100)]
		[Display(Name = "Assigned Barangay")]
		public string AssignedBarangay { get; set; } = string.Empty;

		[Phone]
		[Display(Name = "Contact Number")]
		public string? ContactNumber { get; set; }

		[EmailAddress]
		public string? Email { get; set; }

		[Display(Name = "Active")]
		public bool IsActive { get; set; } = true;

		[Display(Name = "Date Hired")]
		[DataType(DataType.Date)]
		public DateTime DateHired { get; set; } = DateTime.Today;

		// Navigation
		public ICollection<Appointment>? Appointments { get; set; }
		public ICollection<Consultation>? Consultations { get; set; }

		public string FullName => $"{FirstName} {LastName}";
	}
}