using Microsoft.Extensions.Configuration;
using System;
using TvMazeScraper.Infrastructure.Interface;
using TvMazeScraper.Infrastructure.Interface.Interfaces;

namespace TvMazeScraper.Infrastructure.Services
{
	public class StringsProvider : IStringsProvider
	{
		private const string CONFIG_STRINGS_SECTION_NAME = "strings";

		private readonly IConfigurationSection _strings;


		public StringsProvider(IConfiguration configuration)
		{
			_strings = configuration.GetSection(CONFIG_STRINGS_SECTION_NAME);
		}


		public string this[StringsEnum key]
		{
			get { return _strings[Enum.GetName(typeof(StringsEnum), key)]; }
		}
	}
}
