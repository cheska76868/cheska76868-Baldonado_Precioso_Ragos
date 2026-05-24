using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarangayTelemedicine.Models
{
	public class Appointment
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Patient is required.")]
		[Display(Name = "Patient")]
		public int PatientId { get; set; }

		[ForeignKey("PatientId")]
		public Patient? Patient { get; set; }

		[Required(ErrorMessage = "Health worker is required.")]
		[Display(Name = "Health Worker")]
		public int HealthWorkerId { get; set; }

		[ForeignKey("HealthWorkerId")]
		public HealthWorker? HealthWorker { get; set; }

		[Required(ErrorMessage = "Appointment date is required.")]
		[DataType(DataType.DateTime)]
		[Display(Name = "Appointment Date & Time")]
		public DateTime AppointmentDate { get; set; }

		[Required(ErrorMessage = "Reason is required.")]
		[StringLength(300, ErrorMessage = "Reason cannot exceed 300 characters.")]
		[Display(Name = "Reason for Visit")]
		public string Reason { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled

		[StringLength(500)]
		public string? Notes { get; set; }

		[Display(Name = "Created On")]
		public DateTime CreatedOn { get; set; } = DateTime.Now;

		// Navigation
		public Consultation? Consultation { get; set; }
	}
}