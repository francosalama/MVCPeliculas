using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Context;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVCPeliculas.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MVCPeliculas
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			services.AddDbContext<PeliculaDatabaseContext>(options => options.UseSqlServer(Configuration["ConnectionString:PeliculaDBConnection"]));
			services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
			.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                    options =>
                    {
                        options.LoginPath = "/Login";
                        options.AccessDeniedPath = "/AccesoDenegado";
                    }
            );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(Rol.Admin.ToString()));
                options.AddPolicy("UserOnly", policy => policy.RequireRole(Rol.Usuario.ToString()));
                options.AddPolicy("UserIdPolicy", policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
            });
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",	
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

            app.UseCookiePolicy();
        }
	}
}
