using BarangayTelemedicine.Data;
using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize]
	public class PatientsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public PatientsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: /Patients — with search
		public async Task<IActionResult> Index(string? search, string? barangay)
		{
			var query = _context.Patients.AsQueryable();

			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(p =>
					p.FirstName.Contains(search) || p.LastName.Contains(search));

			if (!string.IsNullOrWhiteSpace(barangay))
				query = query.Where(p => p.Barangay.Contains(barangay));

			ViewBag.Search = search;
			ViewBag.Barangay = barangay;

			return View(await query.OrderByDescending(p => p.RegisteredOn).ToListAsync());
		}

		// GET: /Patients/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var patient = await _context.Patients
				.Include(p => p.Appointments)
					.ThenInclude(a => a.HealthWorker)
				.Include(p => p.Consultations)
					.ThenInclude(c => c.HealthWorker)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (patient == null) return NotFound();
			return View(patient);
		}

		// GET: /Patients/Create
		[Authorize(Roles = "Admin,HealthWorker")]
		public IActionResult Create() => View();

		// POST: /Patients/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Create(Patient patient)
		{
			if (ModelState.IsValid)
			{
				patient.RegisteredOn = DateTime.Now;
				_context.Add(patient);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Patient registered successfully.";
				return RedirectToAction(nameof(Index));
			}
			return View(patient);
		}

		// GET: /Patients/Edit/5
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id)
		{
			var patient = await _context.Patients.FindAsync(id);
			if (patient == null) return NotFound();
			return View(patient);
		}

		// POST: /Patients/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,HealthWorker")]
		public async Task<IActionResult> Edit(int id, Patient patient)
		{
			if (id != patient.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(patient);
					await _context.SaveChangesAsync();
					TempData["Success"] = "Patient updated successfully.";
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Patients.Any(p => p.Id == id)) return NotFound();
					throw;
				}
			}
			return View(patient);
		}

		// GET: /Patients/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var patient = await _context.Patients.FindAsync(id);
			if (patient == null) return NotFound();
			return View(patient);
		}

		// POST: /Patients/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var patient = await _context.Patients.FindAsync(id);
			if (patient != null)
			{
				_context.Patients.Remove(patient);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Patient deleted successfully.";
			}
			return RedirectToAction(nameof(Index));
		}
	}
}