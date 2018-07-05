using ApiServer.Middleware;
using AutoMapper;
using BusinessLogic;
using Database;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer
{
	public class Startup
	{
		private const string ERROR_PAGES_TEMPLATE = "/errors/{0}.html";

		public IConfiguration Configuration { get; }


		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}


		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddAutoMapper()
				.ConfigureInfrastructureServices()
				.ConfigureDatabaseServices(Configuration)
				.ConfigureBusinessLogicServices();

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

			app.UseStatusCodePagesWithReExecute(ERROR_PAGES_TEMPLATE);
			// Uncomment if HTTPS required
			//app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
