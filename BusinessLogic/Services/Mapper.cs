using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DataModels;
using BusinessLogic.Interfaces;
using Database.Models;
using Server.Models;

namespace BusinessLogic.Services
{
	public class Mapper : IMapper
	{
		private readonly Dictionary<Type, Dictionary<Type, Func<object, object>>> _fromTypeToTypeMappings;


		public Mapper()
		{
			_fromTypeToTypeMappings = new Dictionary<Type, Dictionary<Type, Func<object, object>>>();

			RegisterMapping<Show, ApiShow>(Map);
			RegisterMapping<Cast, ApiCast>(Map);
			RegisterMapping<ApiShow, Show>(Map);
			RegisterMapping<ApiCast, Cast>(Map);
			RegisterMapping<TvMazeShowData, ApiShow>(Map);
			RegisterMapping<TvMazeShowData, Show>(MapTvMazeShowDataToShow);
			RegisterMapping<TvMazerPerson, Cast>(Map);
		}


		public TTarget Map<TSource, TTarget>(TSource source) 
			where TTarget : class
			where TSource : class 
		{
			if (source == null)
			{
				return null;
			}

			var typeFrom = typeof(TSource);
			var typeTo = typeof(TTarget);

			EnsureMappingExists(typeFrom, typeTo);

			return _fromTypeToTypeMappings[typeFrom][typeTo].Invoke(source) as TTarget;
		}

		public IEnumerable<TTarget> MapCollection<TSource, TTarget>(IEnumerable<TSource> source)
			where TTarget : class
			where TSource : class
		{
			if (source == null)
			{
				return Enumerable.Empty<TTarget>();
			}

			var typeFrom = typeof(TSource);
			var typeTo = typeof(TTarget);

			EnsureMappingExists(typeFrom, typeTo);

			return source.Select(item => _fromTypeToTypeMappings[typeFrom][typeTo].Invoke(item) as TTarget).ToList();
		}


		private void RegisterMapping<TFrom, TTo>(Func<TFrom, TTo> mapper) where TFrom : class
		{
			var fromType = typeof(TFrom);
			var toType = typeof(TTo);

			if (!_fromTypeToTypeMappings.ContainsKey(fromType))
			{
				_fromTypeToTypeMappings.Add(fromType, new Dictionary<Type, Func<object, object>>());
			}

			if (!_fromTypeToTypeMappings[fromType].ContainsKey(toType))
			{
				_fromTypeToTypeMappings[fromType].Add(toType, null);
			}

			_fromTypeToTypeMappings[fromType][toType] = data => mapper.Invoke(data as TFrom);
		}

		private void EnsureMappingExists(Type typeFrom, Type typeTo)
		{
			if (!_fromTypeToTypeMappings.ContainsKey(typeFrom) || !_fromTypeToTypeMappings[typeFrom].ContainsKey(typeTo))
			{
				throw new ArgumentException(
					$"Cannot map object. Mappings for {typeFrom.FullName} -> {typeTo.FullName} haven't been created.");
			}
		}

		private ApiShow Map(Show data)
		{
			return data == null
				? null
				: new ApiShow
				{
					Id = data.Id,
					Name = data.Name,
					Casts = MapCollection<Cast, ApiCast>(data.Casts)
				};
		}

		private Show Map(ApiShow data)
		{
			return data == null
				? null
				: new Show
				{
					Id = data.Id,
					Name = data.Name,
					Casts = MapCollection<ApiCast, Cast>(data.Casts)?.ToList()
				};
		}

		private ApiCast Map(Cast data)
		{
			return data == null
				? null
				: new ApiCast
				{
					Id = data.Id,
					Name = data.Name,
					Birthday = data.Birthday
				};
		}

		private Cast Map(ApiCast data)
		{
			return data == null
				? null
				: new Cast
				{
					Id = data.Id,
					Name = data.Name,
					Birthday = data.Birthday
				};
		}

		private ApiShow Map(TvMazeShowData data)
		{
			return data == null
				? null
				: new ApiShow
				{
					Id = data.Id,
					Name = data.Name
				};
		}

		private Cast Map(TvMazerPerson data)
		{
			return data == null
				? null
				: new Cast
				{
					Id = data.Id,
					Name = data.Name,
					Birthday = data.Birthday == null ? (DateTime?) null : DateTime.Parse(data.Birthday)
				};
		}

		private Show MapTvMazeShowDataToShow(TvMazeShowData data)
		{
			return data == null
				? null
				: new Show
				{
					Id = data.Id,
					Name = data.Name,
					Casts = MapCollection<TvMazerPerson, Cast>(data.Casts).ToList()
				};
		}
	}
}