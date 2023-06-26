using AutoMapper;
using Dapper.Core.Entities;
using Dapper.WepApi.Models.Dto;

namespace Dapper.WepApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserDto, User>();
                config.CreateMap<User, UserDto>();
            });

            return mappingConfig;
        }
    }
}
