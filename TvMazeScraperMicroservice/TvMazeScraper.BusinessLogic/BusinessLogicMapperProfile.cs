using AutoMapper;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Models;

namespace TvMazeScraper.BusinessLogic
{
	public class BusinessLogicMapperProfile : Profile
	{
		public BusinessLogicMapperProfile()
		{
			CreateMap<TvMazeCastData, TvMazeCastModel>();
			CreateMap<TvMazeShowData, TvMazeShowModel>();
			CreateMap<TvMazeCastModel, CastData>();
			CreateMap<TvMazeShowModel, ShowData>();
		}
	}
}