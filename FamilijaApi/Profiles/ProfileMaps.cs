using FamilijaApi.DTOs;
using FamilijaApi.Models;
using AutoMapper;

namespace FamilijaApi.Profiles
{
    public class ProfileMaps : Profile
    {
        public ProfileMaps(){
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();

            CreateMap<Address, AddressReadDto>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>();
            CreateMap<Address, AddressUpdateDto>();

            CreateMap<Contact, ContactReadDto>();
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();
            CreateMap<Contact, ContactUpdateDto>();

            CreateMap<PersonalInfo, PersonalInfoReadDto>();
            CreateMap<PersonalInfoCreateDto, PersonalInfo>();
            CreateMap<PersonalInfoUpdateDtos, PersonalInfo>();
            CreateMap<PersonalInfo, PersonalInfoUpdateDtos>();

            CreateMap<UserInfo, UserInfoReadDto>();
            CreateMap<UserInfoCreateDto, UserInfo>();
            CreateMap<UserUpdateDto, UserInfo>();
            CreateMap<UserInfo, UserUpdateDto>();
        }
    }
}