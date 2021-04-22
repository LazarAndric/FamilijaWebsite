using FamilijaApi.DTOs;
using FamilijaApi.Models;
using AutoMapper;

namespace FamilijaApi.Profiles
{
    public class ProfileMaps : Profile
    {
        public ProfileMaps(){
            CreateMap<User, UserReadDto>();

            CreateMap<Address, AddressReadDto>();

            CreateMap<Contact, ContactReadDto>();

            CreateMap<PersonalInfo, PersonalInfoReadDto>();

            CreateMap<UserInfo, UserInfoReadDto>();
        }
    }
}