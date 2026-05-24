using BarangayTelemedicine.Models;
using BarangayTelemedicine.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BarangayTelemedicine.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
								  SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		// GET: /Account/Login
		public IActionResult Login() => View();

		// POST: /Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _signInManager.PasswordSignInAsync(
				model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
				return RedirectToAction("Index", "Dashboard");

			ModelState.AddModelError("", "Invalid email or password.");
			return View(model);
		}

		// GET: /Account/Register
		public IActionResult Register() => View();

		// POST: /Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				FullName = model.FullName,
				Role = model.Role
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.Role);
				await _signInManager.SignInAsync(user, isPersistent: false);
				return RedirectToAction("Index", "Dashboard");
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError("", error.Description);

			return View(model);
		}

		// POST: /Account/Logout
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}

		public IActionResult AccessDenied() => View();
	}
}