using BarangayTelemedicine.Data;
using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayTelemedicine.Controllers
{
	[Authorize(Roles = "Admin,HealthWorker")]
	public class MedicinesController : Controller
	{
		private readonly ApplicationDbContext _context;
		public MedicinesController(ApplicationDbContext context) => _context = context;

		public async Task<IActionResult> Index(string? search, string? category)
		{
			var query = _context.Medicines.AsQueryable();
			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(m => m.Name.Contains(search) || m.GenericName.Contains(search));
			if (!string.IsNullOrWhiteSpace(category))
				query = query.Where(m => m.Category == category);
			ViewBag.Search = search;
			ViewBag.Category = category;
			ViewBag.Categories = _context.Medicines.Select(m => m.Category).Distinct().ToList();
			return View(await query.OrderBy(m => m.Name).ToListAsync());
		}

		public async Task<IActionResult> Details(int id)
		{
			var m = await _context.Medicines.FindAsync(id);
			if (m == null) return NotFound();
			return View(m);
		}

		public IActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Medicine medicine)
		{
			if (ModelState.IsValid)
			{
				_context.Add(medicine);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Medicine added to inventory.";
				return RedirectToAction(nameof(Index));
			}
			return View(medicine);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var m = await _context.Medicines.FindAsync(id);
			if (m == null) return NotFound();
			return View(m);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Medicine medicine)
		{
			if (id != medicine.Id) return NotFound();
			if (ModelState.IsValid)
			{
				_context.Update(medicine);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Medicine updated.";
				return RedirectToAction(nameof(Index));
			}
			return View(medicine);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var m = await _context.Medicines.FindAsync(id);
			if (m == null) return NotFound();
			return View(m);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var m = await _context.Medicines.FindAsync(id);
			if (m != null) { _context.Medicines.Remove(m); await _context.SaveChangesAsync(); }
			TempData["Success"] = "Medicine removed.";
			return RedirectToAction(nameof(Index));
		}
	}
}
