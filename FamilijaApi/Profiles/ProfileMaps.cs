using FamilijaApi.DTOs;
using FamilijaApi.Models;
using AutoMapper;

namespace FamilijaApi.Profiles
{
    public class ProfileMaps : Profile
    {
        public ProfileMaps(){
            CreateMap<User, UserReadDTO>();
        }
    }
}