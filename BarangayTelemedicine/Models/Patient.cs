using System.ComponentModel.DataAnnotations;

namespace BarangayTelemedicine.Models
{
	public class Patient
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "First name is required.")]
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name is required.")]
		[StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Date of birth is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Date of Birth")]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Gender is required.")]
		[StringLength(10)]
		public string Gender { get; set; } = string.Empty;

		[Required(ErrorMessage = "Barangay is required.")]
		[StringLength(100, ErrorMessage = "Barangay name cannot exceed 100 characters.")]
		public string Barangay { get; set; } = string.Empty;

		[StringLength(200)]
		public string? Address { get; set; }

		[Phone(ErrorMessage = "Invalid phone number.")]
		[Display(Name = "Contact Number")]
		public string? ContactNumber { get; set; }

		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string? Email { get; set; }

		[Display(Name = "Registered On")]
		public DateTime RegisteredOn { get; set; } = DateTime.Now;

		[StringLength(500)]
		[Display(Name = "Medical Notes")]
		public string? MedicalNotes { get; set; }

		// Navigation
		public ICollection<Appointment>? Appointments { get; set; }
		public ICollection<Consultation>? Consultations { get; set; }

		// Computed
		public string FullName => $"{FirstName} {LastName}";
		public int Age => DateTime.Today.Year - DateOfBirth.Year -
						  (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
	}
}