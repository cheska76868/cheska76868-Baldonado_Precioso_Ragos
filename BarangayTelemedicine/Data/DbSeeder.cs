using BarangayTelemedicine.Models;
using Microsoft.AspNetCore.Identity;

namespace BarangayTelemedicine.Data
{
	public static class DbSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Seed roles
			string[] roles = { "Admin", "HealthWorker", "Patient" };
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new IdentityRole(role));
			}

			// Seed admin user
			var adminEmail = "admin@barangay.gov.ph";
			if (await userManager.FindByEmailAsync(adminEmail) == null)
			{
				var admin = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					FullName = "System Administrator",
					Role = "Admin",
					EmailConfirmed = true
				};
				IdentityResult result = await userManager.CreateAsync(admin, "Admin@123");
				if (result.Succeeded)
					await userManager.AddToRoleAsync(admin, "Admin");
			}
		}
	}
}