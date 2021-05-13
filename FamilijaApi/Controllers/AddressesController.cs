using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;
using System.Threading.Tasks;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using FamilijaApi.Utility;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Configuration;
using Microsoft.Extensions.Options;
using FamilijaApi.Data;
using Microsoft.IdentityModel.Tokens;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AddressesController : ControllerBase
    {
        private IAdddressesRepo _addressRepo;
        private IMapper _mapper;
        private readonly IJwtUtil _jwtTokenUtil;

        public AddressesController (IRoleRepo roleRepo, IAuthRepo authRepo, IUserRepo userRepo, IAdddressesRepo adddressesRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _addressRepo = adddressesRepo;
            _mapper = mapper;
            _jwtTokenUtil=new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAddress([FromBody] TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){            
                var content = await _addressRepo.GetAddress(auth.Id);
                if (content == null) 
                    return NotFound(finalAuth);
                return Ok(new CommunicationModel<AddressReadDto>(){
                    GenericModel=_mapper.Map<AddressReadDto>(content),
                    AuthResult=finalAuth
                });
            }
            return Unauthorized(finalAuth);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAddress([FromBody] AddressCreateDto addressCreateDto)
        //{
        //    var addres = _mapper.Map<Address>(addressCreateDto);
        //    _addressRepo.CreateAddress(addres);
        //    await _addressRepo.SaveChanges();

        //    return Created("", addres);
        //}

        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] GetCommunicationModel<AddressUpdateDto> data)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(data.TokenRequest);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){ 
                var updateModelAddress = _addressRepo.GetAddress(auth.Id).Result;
                if (updateModelAddress == null)
                {
                    finalAuth.Errors.Add("Addres is not found");
                    return NotFound(finalAuth);
                }
                _mapper.Map(data.GenericModel, updateModelAddress);
                _addressRepo.UpdateAddress(updateModelAddress);
                await _addressRepo.SaveChanges();
                return Ok(finalAuth);
            }
            finalAuth.Errors.Add("Unauthorized");
            return Unauthorized();


        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAddress([FromBody] TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var deleteModelAddress = _addressRepo.GetAddress(auth.Id).Result;
                if (deleteModelAddress == null)
                {
                    finalAuth.Errors.Add("User not found");
                    return NotFound(finalAuth);
                }
                _addressRepo.DeleteAdress(deleteModelAddress);
                await _addressRepo.SaveChanges();
                return NoContent();
            }
            finalAuth.Errors.Add("Unauthorized");
            return Unauthorized();
        }
    }
}
