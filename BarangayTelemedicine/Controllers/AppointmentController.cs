using BarangayTelemedicine.Data;
using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize]
	public class AppointmentsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AppointmentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: /Appointments — with filter
		public async Task<IActionResult> Index(string? status, DateTime? date)
		{
			var query = _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.HealthWorker)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(status))
				query = query.Where(a => a.Status == status);

			if (date.HasValue)
				query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

			ViewBag.Status = status;
			ViewBag.Date = date?.ToString("yyyy-MM-dd");
			ViewBag.StatusList = new SelectList(new[] { "Pending", "Confirmed", "Completed", "Cancelled" });

			return View(await query.OrderByDescending(a => a.AppointmentDate).ToListAsync());
		}

		// GET: /Appointments/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var appointment = await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.HealthWorker)
				.Include(a => a.Consultation)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (appointment == null) return NotFound();
			return View(appointment);
		}

		// GET: /Appointments/Create
		[Authorize(Roles = "Admin,HealthWorker")]
		public IActionResult Create()
		{
			PopulateDropdowns();
			return View();
		}

		// POST: /Appointments/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Create(Appointment appointment)
		{
			if (ModelState.IsValid)
			{
				appointment.CreatedOn = DateTime.Now;
				_context.Add(appointment);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Appointment scheduled successfully.";
				return RedirectToAction(nameof(Index));
			}
			PopulateDropdowns(appointment.PatientId, appointment.HealthWorkerId);
			return View(appointment);
		}

		// GET: /Appointments/Edit/5
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment == null) return NotFound();
			PopulateDropdowns(appointment.PatientId, appointment.HealthWorkerId);
			ViewBag.StatusList = new SelectList(new[] { "Pending", "Confirmed", "Completed", "Cancelled" }, appointment.Status);
			return View(appointment);
		}

		// POST: /Appointments/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id, Appointment appointment)
		{
			if (id != appointment.Id) return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(appointment);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Appointment updated successfully.";
				return RedirectToAction(nameof(Index));
			}
			PopulateDropdowns(appointment.PatientId, appointment.HealthWorkerId);
			ViewBag.StatusList = new SelectList(new[] { "Pending", "Confirmed", "Completed", "Cancelled" }, appointment.Status);
			return View(appointment);
		}

		// GET: /Appointments/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var appointment = await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.HealthWorker)
				.FirstOrDefaultAsync(a => a.Id == id);
			if (appointment == null) return NotFound();
			return View(appointment);
		}

		// POST: /Appointments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment != null)
			{
				_context.Appointments.Remove(appointment);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Appointment deleted.";
			}
			return RedirectToAction(nameof(Index));
		}

		private void PopulateDropdowns(int? patientId = null, int? healthWorkerId = null)
		{
			ViewBag.Patients = new SelectList(_context.Patients.ToList(), "Id", "FullName", patientId);
			ViewBag.HealthWorkers = new SelectList(_context.HealthWorkers.Where(h => h.IsActive).ToList(), "Id", "FullName", healthWorkerId);
		}
	}
}