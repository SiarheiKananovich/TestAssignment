﻿namespace Infrastructure.Interface.Interfaces
{
	public interface IStringsProvider
	{
		string this[StringsEnum key] { get; }
	}
}