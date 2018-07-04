using ApiServer.Models;
using AutoMapper;
using BusinessLogic.Interface.Models;

namespace ApiServer
{
    public class ApiServerMapperProfile : Profile
	{
		public ApiServerMapperProfile()
		{
			CreateMap<ShowModel, ApiShow>();
		}
    }
}
