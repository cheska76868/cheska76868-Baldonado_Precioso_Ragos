using BarangayTelemedicine.Data;
using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize(Roles = "Admin")]
	public class HealthWorkersController : Controller
	{
		private readonly ApplicationDbContext _context;
		public HealthWorkersController(ApplicationDbContext context) => _context = context;

		public async Task<IActionResult> Index(string? search)
		{
			var query = _context.HealthWorkers.AsQueryable();
			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(h => h.FirstName.Contains(search) || h.LastName.Contains(search) || h.AssignedBarangay.Contains(search));
			ViewBag.Search = search;
			return View(await query.OrderBy(h => h.LastName).ToListAsync());
		}

		public async Task<IActionResult> Details(int id)
		{
			var hw = await _context.HealthWorkers.FindAsync(id);
			if (hw == null) return NotFound();
			return View(hw);
		}

		public IActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(HealthWorker hw)
		{
			if (ModelState.IsValid)
			{
				_context.Add(hw);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Health worker added.";
				return RedirectToAction(nameof(Index));
			}
			return View(hw);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var hw = await _context.HealthWorkers.FindAsync(id);
			if (hw == null) return NotFound();
			return View(hw);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, HealthWorker hw)
		{
			if (id != hw.Id) return NotFound();
			if (ModelState.IsValid)
			{
				_context.Update(hw);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Health worker updated.";
				return RedirectToAction(nameof(Index));
			}
			return View(hw);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var hw = await _context.HealthWorkers.FindAsync(id);
			if (hw == null) return NotFound();
			return View(hw);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var hw = await _context.HealthWorkers.FindAsync(id);
			if (hw != null) { _context.HealthWorkers.Remove(hw); await _context.SaveChangesAsync(); }
			TempData["Success"] = "Health worker removed.";
			return RedirectToAction(nameof(Index));
		}
	}
}