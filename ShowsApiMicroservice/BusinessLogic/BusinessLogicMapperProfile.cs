using AutoMapper;
using BusinessLogic.Interface.Models;
using Database.Interface.Models;

namespace BusinessLogic
{
	public class BusinessLogicMapperProfile : Profile
	{
		public BusinessLogicMapperProfile()
		{
			CreateMap<Show, ShowModel>();
			CreateMap<Cast, CastModel>();
			CreateMap<ShowModel, Show>();
			CreateMap<CastModel, Cast>();
		}
	}
}