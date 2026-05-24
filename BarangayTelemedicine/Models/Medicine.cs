using System.ComponentModel.DataAnnotations;

namespace BarangayTelemedicine.Models
{
	public class Medicine
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Medicine name is required.")]
		[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
		[Display(Name = "Medicine Name")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Generic name is required.")]
		[StringLength(100)]
		[Display(Name = "Generic Name")]
		public string GenericName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Category is required.")]
		[StringLength(50)]
		public string Category { get; set; } = string.Empty; // Antibiotic, Analgesic, etc.

		[StringLength(50)]
		public string? Dosage { get; set; } // e.g., 500mg

		[StringLength(20)]
		[Display(Name = "Unit")]
		public string? Unit { get; set; } // tablet, capsule, bottle

		[Required(ErrorMessage = "Stock quantity is required.")]
		[Range(0, 100000, ErrorMessage = "Stock must be between 0 and 100,000.")]
		[Display(Name = "Stock Quantity")]
		public int StockQuantity { get; set; }

		[Display(Name = "Expiry Date")]
		[DataType(DataType.Date)]
		public DateTime? ExpiryDate { get; set; }

		[StringLength(300)]
		public string? Description { get; set; }

		[Display(Name = "Requires Prescription")]
		public bool RequiresPrescription { get; set; } = false;

		[Display(Name = "Stock Status")]
		public string StockStatus => StockQuantity == 0 ? "Out of Stock" :
									 StockQuantity < 20 ? "Low Stock" : "Available";
	}
}