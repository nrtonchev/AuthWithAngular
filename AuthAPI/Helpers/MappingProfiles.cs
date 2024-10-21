using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace AuthAPI.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<User, AuthResponse>();
            CreateMap<AuthResponse, User>();
        }
    }
}
