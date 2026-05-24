using BarangayTelemedicine.Data;
using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize]
	public class ConsultationsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ConsultationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: /Consultations
		public async Task<IActionResult> Index(DateTime? date, string? search)
		{
			var query = _context.Consultations
				.Include(c => c.Patient)
				.Include(c => c.HealthWorker)
				.AsQueryable();

			if (date.HasValue)
				query = query.Where(c => c.ConsultationDate.Date == date.Value.Date);

			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(c =>
					c.Patient!.FirstName.Contains(search) || c.Patient.LastName.Contains(search));

			ViewBag.Date = date?.ToString("yyyy-MM-dd");
			ViewBag.Search = search;

			return View(await query.OrderByDescending(c => c.ConsultationDate).ToListAsync());
		}

		// GET: /Consultations/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var consultation = await _context.Consultations
				.Include(c => c.Patient)
				.Include(c => c.HealthWorker)
				.Include(c => c.Appointment)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (consultation == null) return NotFound();
			return View(consultation);
		}

		// GET: /Consultations/Create
		[Authorize(Roles = "Admin,HealthWorker")]
		public IActionResult Create()
		{
			PopulateDropdowns();
			return View(new Consultation { ConsultationDate = DateTime.Today });
		}

		// POST: /Consultations/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Create(Consultation consultation)
		{
			if (ModelState.IsValid)
			{
				_context.Add(consultation);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Consultation record saved successfully.";
				return RedirectToAction(nameof(Index));
			}
			PopulateDropdowns(consultation.PatientId, consultation.HealthWorkerId);
			return View(consultation);
		}

		// GET: /Consultations/Edit/5
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id)
		{
			var consultation = await _context.Consultations.FindAsync(id);
			if (consultation == null) return NotFound();
			PopulateDropdowns(consultation.PatientId, consultation.HealthWorkerId);
			return View(consultation);
		}

		// POST: /Consultations/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id, Consultation consultation)
		{
			if (id != consultation.Id) return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(consultation);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Consultation record updated.";
				return RedirectToAction(nameof(Index));
			}
			PopulateDropdowns(consultation.PatientId, consultation.HealthWorkerId);
			return View(consultation);
		}

		// GET: /Consultations/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var consultation = await _context.Consultations
				.Include(c => c.Patient)
				.Include(c => c.HealthWorker)
				.FirstOrDefaultAsync(c => c.Id == id);
			if (consultation == null) return NotFound();
			return View(consultation);
		}

		// POST: /Consultations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var c = await _context.Consultations.FindAsync(id);
			if (c != null)
			{
				_context.Consultations.Remove(c);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Consultation record deleted.";
			}
			return RedirectToAction(nameof(Index));
		}

		private void PopulateDropdowns(int? patientId = null, int? healthWorkerId = null)
		{
			ViewBag.Patients = new SelectList(_context.Patients.ToList(), "Id", "FullName", patientId);
			ViewBag.HealthWorkers = new SelectList(_context.HealthWorkers.Where(h => h.IsActive).ToList(), "Id", "FullName", healthWorkerId);
			ViewBag.Appointments = new SelectList(_context.Appointments.Include(a => a.Patient).ToList(), "Id", "Reason", null);
		}
	}
}