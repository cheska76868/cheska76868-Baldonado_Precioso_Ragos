using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarangayTelemedicine.Models
{
	public class Consultation
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Patient is required.")]
		[Display(Name = "Patient")]
		public int PatientId { get; set; }

		[ForeignKey("PatientId")]
		public Patient? Patient { get; set; }

		[Display(Name = "Appointment")]
		public int? AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public Appointment? Appointment { get; set; }

		[Required(ErrorMessage = "Health Worker is required.")]
		[Display(Name = "Health Worker")]
		public int HealthWorkerId { get; set; }

		[ForeignKey("HealthWorkerId")]
		public HealthWorker? HealthWorker { get; set; }

		[Required(ErrorMessage = "Consultation date is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Consultation Date")]
		public DateTime ConsultationDate { get; set; } = DateTime.Today;

		[Required(ErrorMessage = "Chief complaint is required.")]
		[StringLength(500, ErrorMessage = "Chief complaint cannot exceed 500 characters.")]
		[Display(Name = "Chief Complaint")]
		public string ChiefComplaint { get; set; } = string.Empty;

		[Required(ErrorMessage = "Diagnosis is required.")]
		[StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters.")]
		public string Diagnosis { get; set; } = string.Empty;

		[StringLength(1000)]
		public string? Prescription { get; set; }

		[StringLength(1000)]
		[Display(Name = "Treatment Plan")]
		public string? TreatmentPlan { get; set; }

		[StringLength(200)]
		[Display(Name = "Vital Signs")]
		public string? VitalSigns { get; set; } // e.g., BP: 120/80, Temp: 36.5

		[Display(Name = "Follow-up Date")]
		[DataType(DataType.Date)]
		public DateTime? FollowUpDate { get; set; }

		public bool IsTelemedicine { get; set; } = false;

		[Display(Name = "Telemedicine")]
		public string TelemedicineLabel => IsTelemedicine ? "Yes (Remote)" : "In-Person";
	}
}