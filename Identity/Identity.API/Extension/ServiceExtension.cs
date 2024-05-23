using Identity.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Extension;

public static class ServiceExtension
{
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		var builder = services.AddIdentity<ApplicationUser, IdentityRole>(o =>
			{
				o.Password.RequireDigit = true;
				o.Password.RequireLowercase = false;
				o.Password.RequireUppercase = false;
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequiredLength = 10;
				o.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();
	}
}