﻿using Infrastructure.Interface.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureConfigureExtension
	{
		public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
		{
			services.AddSingleton<IStringsProvider, StringsProvider>();

			return services;
		}
	}
}