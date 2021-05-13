using FamilijaApi.DTOs;
using FamilijaApi.Models;
using AutoMapper;
using System.Threading.Tasks;
using FamilijaApi.Configuration;

namespace FamilijaApi.Profiles
{
    public class ProfileMaps : Profile
    {
        public ProfileMaps(){
            CreateMap<AuthCreate, AuthResult>();
            CreateMap<AuthResult, AuthCreate>();

            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<User, PasswordReadDto>();

            CreateMap<ConfirmedEmailDto, User>();
            CreateMap<User, ConfirmedEmailDto>();

            CreateMap<Address, AddressReadDto>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>();
            CreateMap<Address, AddressUpdateDto>();

            CreateMap<PersonalInfo, PersonalInfoReadDto>();
            CreateMap<PersonalInfoCreateDto, PersonalInfo>();
            CreateMap<PersonalInfoUpdateDtos, PersonalInfo>();
            CreateMap<PersonalInfo, PersonalInfoUpdateDtos>();

            CreateMap<Finance, FinanceReadDto>();
            CreateMap<FinanceCreateDto, Finance>();
            CreateMap<FinanceUpdateDto, Finance>();
            CreateMap<Finance, FinanceUpdateDto>();

            CreateMap<Role, RoleReadDto>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<Role, RoleUpdateDto>();

            CreateMap<Password, PasswordReadDto>();
            CreateMap<PasswordCreateDto, Password>();
            CreateMap<PasswordUpdateDto, Password>();
            CreateMap<Password, RoleUpdateDto>();
        }
    }
}