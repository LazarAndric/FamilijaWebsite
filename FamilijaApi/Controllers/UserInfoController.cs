using System.Threading.Tasks;
using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FamilijaAPi.Coontrollers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserInfoController : ControllerBase
    {
        private IUserInfoRepo _userInfoRepo;
        private IUserRepo _userRepo;
        private IRoleRepo _roleRepo;
        private IMapper _mapper;

        public UserInfoController(IUserInfoRepo userInfoRepo, IMapper mapper, IUserRepo userRepo, IRoleRepo roleRepo)
        {
            _mapper = mapper;
            _userInfoRepo=userInfoRepo;
            _userRepo=userRepo;
            _roleRepo=roleRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoReadDto>> GetUserInfo(int id)
        {
            var content=await _userInfoRepo.GetUserInfo(id);
            if(content==null)   return NoContent();
            var refUser= await _userRepo.GetUserById(content.ReferralId);
            if(refUser!=null) content.ReferralUser=refUser;
            var role=await _roleRepo.GetRole(content.RoleId);
            if(role!=null)  content.Role=role;
            return Ok(_mapper.Map<UserInfo, UserInfoReadDto>(content));
        }

    }
}