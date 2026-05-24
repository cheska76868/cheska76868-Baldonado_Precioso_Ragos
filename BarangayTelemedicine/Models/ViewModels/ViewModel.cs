using System.ComponentModel.DataAnnotations;

namespace BarangayTelemedicine.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Full name is required.")]
		[StringLength(100)]
		[Display(Name = "Full Name")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please confirm your password.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		public string Role { get; set; } = "Patient";
	}

	public class DashboardViewModel
	{
		public int TotalPatients { get; set; }
		public int TotalAppointments { get; set; }
		public int TotalConsultations { get; set; }
		public int TotalHealthWorkers { get; set; }
		public int PendingAppointments { get; set; }
		public int TelemedicineConsultations { get; set; }
		public List<RecentAppointment> RecentAppointments { get; set; } = new();
	}

	public class RecentAppointment
	{
		public string PatientName { get; set; } = string.Empty;
		public string HealthWorker { get; set; } = string.Empty;
		public DateTime Date { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}