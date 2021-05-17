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
        public async Task<IActionResult> GetPersonalInfo([FromHeader] string authorization)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){
                    var content = await _personalInfoRepo.GetPersonalInfoAsync(auth.User.Id);
                    if (content == null)
                        return NotFound("Personal info of user is not found");
                    return Ok(new CommunicationModel<PersonalInfoReadDto>(){
                        GenericModel=_mapper.Map<PersonalInfoReadDto>(content),
                        Result=new AuthResult(){
                            Success=true
                        }
                    });
                }
                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new AuthResult(){
                    Errors=new List<string>(){
                        ex.Message
                    },
                    Success=false
                });
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo([FromHeader] string authorization, [FromBody] PersonalInfoUpdateDtos infoUpdateDtos)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){
                    var updateModelPersonalInfo = await _personalInfoRepo.GetPersonalInfoAsync(auth.User.Id);
                    if (updateModelPersonalInfo == null)
                    {
                        return NotFound(new AuthResult(){
                            Success=false,
                            Errors=new List<string>(){
                                "Personal info of user is not found"
                                }
                        });
                    }
                    _mapper.Map(infoUpdateDtos, updateModelPersonalInfo);
                    _personalInfoRepo.UpdatePersonalInfo(updateModelPersonalInfo);
                    await _personalInfoRepo.SaveChanges();
                    return Ok(new AuthResult(){
                            Success=true
                        });
                }
                throw new Exception("User is Unauthorize");
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new AuthResult(){
                    Success=false,
                    Errors=new List<string>(){
                        ex.Message
                    }
                });
            }
            
        }

        // [HttpDelete]
        // public async Task<IActionResult> DeletePersonalInfo([FromBody]TokenRequest token)
        // {
        //     var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
        //     var finalAuth= _mapper.Map<AuthResult>(auth);
        //     if(auth.Success){
        //         var deleteModelPersonalInfo = await _personalInfoRepo.GetPersonalInfo(auth.Id);
        //         if (deleteModelPersonalInfo == null)
        //         {
        //             return NotFound(finalAuth);
        //         }
        //         _personalInfoRepo.DeletePersonalInfo(deleteModelPersonalInfo);
        //         await _personalInfoRepo.SaveChanges();
        //         return Ok(finalAuth);
        //     }
        //     return Unauthorized();
        // }
    }
}
