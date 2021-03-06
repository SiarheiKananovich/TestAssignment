﻿using AutoMapper;
using TvMazeScraper.BusinessLogic.DataModels;
using TvMazeScraper.BusinessLogic.Interface.Models;
using TvMazeScraper.Database.Interface.Models;

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
			CreateMap<CastModel, CastData>();
			CreateMap<ShowModel, ShowData>();
			CreateMap<TvMazeCastModel, CastModel>();
			CreateMap<TvMazeShowModel, ShowModel>();
			CreateMap<ImportRequestModel, ImportRequest>();
		}
	}
}