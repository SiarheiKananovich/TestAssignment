using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
	public interface IMapper
	{
		TTarget Map<TSource, TTarget>(TSource source) 
			where TTarget : class
			where TSource : class;

		IEnumerable<TTarget> MapCollection<TSource, TTarget>(IEnumerable<TSource> source)
			where TTarget : class
			where TSource : class;
	}
}