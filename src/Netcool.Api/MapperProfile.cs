using AutoMapper;
using Netcool.Api.Domain.Users;

namespace Netcool.Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserSaveInput, User>();
        }
    }
}