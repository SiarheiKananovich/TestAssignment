using ApiServer.Models;
using AutoMapper;
using BusinessLogic.Interface.Models;

namespace ApiServer
{
    public class ApiServerMapperProfile : Profile
	{
		public ApiServerMapperProfile()
		{
			CreateMap<CastModel, ApiCast>();
			CreateMap<ShowModel, ApiShow>();
			CreateMap<ApiCast, CastModel>();
			CreateMap<ApiShow, ShowModel>();
		}
    }
}
