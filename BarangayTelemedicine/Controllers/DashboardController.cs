using BarangayTelemedicine.Data;
using BarangayTelemedicine.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DashboardController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var recentAppointments = await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.HealthWorker)
				.OrderByDescending(a => a.AppointmentDate)
				.Take(5)
				.Select(a => new RecentAppointment
				{
					PatientName = a.Patient!.FirstName + " " + a.Patient.LastName,
					HealthWorker = a.HealthWorker!.FirstName + " " + a.HealthWorker.LastName,
					Date = a.AppointmentDate,
					Status = a.Status
				})
				.ToListAsync();

			var vm = new DashboardViewModel
			{
				TotalPatients = await _context.Patients.CountAsync(),
				TotalAppointments = await _context.Appointments.CountAsync(),
				TotalConsultations = await _context.Consultations.CountAsync(),
				TotalHealthWorkers = await _context.HealthWorkers.CountAsync(h => h.IsActive),
				PendingAppointments = await _context.Appointments.CountAsync(a => a.Status == "Pending"),
				TelemedicineConsultations = await _context.Consultations.CountAsync(c => c.IsTelemedicine),
				RecentAppointments = recentAppointments
			};

			return View(vm);
		}
	}
}