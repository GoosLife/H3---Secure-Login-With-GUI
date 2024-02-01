using H3___Secure_Login_With_GUI.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;

namespace H3___Secure_Login_With_GUI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();

			// Add distributed memory cache to store session data on the server
			builder.Services.AddDistributedMemoryCache();

			// Add session support
			builder.Services.AddSession(options =>
			{
				// 3. HTTP - Rename the session cookie to be less descriptive
				// 4. HTTP - Default ASP.NET Core settings satisfy the requirements for length and randomness
				options.Cookie.Name = ".ZBC.H3.ID";
				// 8. HTTP - A session can last up to 20 minutes of inactivity, and if it takes more than 1 minute to write or read from the session, it will be auto-terminated
				options.IdleTimeout = TimeSpan.FromMinutes(20);
				options.IOTimeout = TimeSpan.FromMinutes(1);
				options.Cookie.IsEssential = true;
			});

			builder.Services.AddAntiforgery(options =>
			{
				options.Cookie.Name = ".ZBC.H3.AF";
				// Override default X-Frame-Options header to set a stricter policy through middleware
				options.SuppressXFrameOptionsHeader = true;
			});

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
					options.SlidingExpiration = true;
					options.AccessDeniedPath = "/Login";
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// 9. HTTP - Set X-Frame-Options to DENY to prevent clickjacking
			app.Use(async (context, next) =>
			{
				if (context.Response.Headers.ContainsKey("X-Frame-Options"))
					context.Response.Headers["X-Frame-Options"] = "DENY";
				else
					context.Response.Headers.Append("X-Frame-Options", "DENY");

				await next();
			});

			app.UseCookiePolicy(new CookiePolicyOptions
			{
				// 6. HTTP - Set cookie policy to be HTTP only and secure. Samesite is strict, and path is only valid paths
				MinimumSameSitePolicy = SameSiteMode.Strict,
				HttpOnly = HttpOnlyPolicy.Always,
				Secure = CookieSecurePolicy.Always
			});

			app.UseSession();

			app.UseMiddleware<SessionMiddleware>();

			// 5. HTTP - Use HTTP Strict Transport Security (HSTS) to force browsers to use HTTPS
			app.UseHsts();

			// 1. HTTP - The site must be served over HTTPS
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}
