using ApiServer.Middleware;
using AutoMapper;
using BusinessLogic;
using Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddAutoMapper()
				.ConfigureDatabaseServices(Configuration)
				.ConfigureBusinessLogicServices();

			//services.AddHostedService<TvMazeScraper>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseSilentExceptionHandler();
				// Uncomment if HTTPS required
				//app.UseHsts();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}.html");
			// Uncomment if HTTPS required
			//app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
