using Microsoft.AspNetCore.Mvc;
using FamilijaApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FailijaApi.Data;
using AutoMapper;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Utility;
using FamilijaApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FamilijaApi.Data;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonalInfoController : ControllerBase
    {
        private readonly JwtTokenUtility _jwtTokenUtil;
        private IPersonalInfoRepo _personalInfoRepo;
        private IMapper _mapper;

        public PersonalInfoController(IAuthRepo authRepo, IUserRepo userRepo, IRoleRepo roleRepo, IPersonalInfoRepo personalInfoRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _personalInfoRepo = personalInfoRepo;
            _mapper = mapper;
            _jwtTokenUtil= new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalInfo([FromBody]TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var content = await _personalInfoRepo.GetPersonalInfo(auth.Id);
                if (content == null) return NoContent();
                return Ok(new CommunicationModel<PersonalInfoReadDto>(){
                    AuthResult=finalAuth,
                    GenericModel=  _mapper.Map<PersonalInfoReadDto>(content)
                });
            }
            return Unauthorized(finalAuth);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] GetCommunicationModel<PersonalInfoUpdateDtos> data)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(data.TokenRequest);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var updateModelPersonalInfo = await _personalInfoRepo.GetPersonalInfo(auth.Id);
                if (updateModelPersonalInfo == null)
                {
                    return NotFound(finalAuth);
                }
                _mapper.Map(data.GenericModel, updateModelPersonalInfo);
                _personalInfoRepo.UpdatePersonalInfo(updateModelPersonalInfo);
                await _personalInfoRepo.SaveChanges();
                return Ok(finalAuth);
            }
            return Unauthorized(finalAuth);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonalInfo([FromBody]TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var deleteModelPersonalInfo = await _personalInfoRepo.GetPersonalInfo(auth.Id);
                if (deleteModelPersonalInfo == null)
                {
                    return NotFound(finalAuth);
                }
                _personalInfoRepo.DeletePersonalInfo(deleteModelPersonalInfo);
                await _personalInfoRepo.SaveChanges();
                return Ok(finalAuth);
            }
            return Unauthorized(finalAuth);
        }
    }
}
