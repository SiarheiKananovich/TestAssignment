using System;
using Infrastructure.Interface;
using Infrastructure.Interface.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
	public class StringsProvider : IStringsProvider
	{
		private readonly IConfigurationSection _strings;


		public StringsProvider(IConfiguration configuration)
		{
			_strings = configuration.GetSection("strings");
		}


		public string this[StringsEnum key]
		{
			get { return _strings[Enum.GetName(typeof(StringsEnum), key)]; }
		}
	}
}
