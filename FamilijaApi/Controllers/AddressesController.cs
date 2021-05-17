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
using System.Collections.Generic;

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
        public async Task<IActionResult> GetAddress([FromHeader] string authorization)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){            
                    var content = await _addressRepo.GetAddressAsync(auth.User.Id);
                    if (content == null) 
                        return NotFound("User not found");
                    return Ok(new CommunicationModel<AddressReadDto>(){
                        Result= new AuthResult(){
                            Success=true
                        },
                        GenericModel=_mapper.Map<AddressReadDto>(content)
                    });
                }
                throw new System.Exception("User is Unauthorize");
            }
            catch (System.Exception ex)
            {
                return Unauthorized(ex.Message);
            }

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
        public async Task<IActionResult> UpdateAddress([FromHeader] string authorization,[FromBody] AddressUpdateDto address)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){ 
                    var updateModelAddress = await _addressRepo.GetAddressAsync(auth.User.Id);
                    if (updateModelAddress == null)
                    {
                        return NotFound("User is not found");
                    }
                    _mapper.Map(address, updateModelAddress);
                    _addressRepo.UpdateAddress(updateModelAddress);
                    await _addressRepo.SaveChanges();
                    return Ok(new AuthResult(){
                        Success=true
                    });
                }
                throw new System.Exception("Unauthorize user");
            }
            catch (System.Exception ex)
            {
                return Ok(new AuthResult(){
                        Success=false,
                        Errors=new List<string>(){
                            ex.Message
                        }
                    });
            }



        }

        // [HttpDelete]
        // public async Task<ActionResult> DeleteAddress([FromHeader] string authorization)
        // {
        //     try
        //     {
        //         var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
        //         if(auth.Success){
        //             var deleteModelAddress = _addressRepo.GetAddressAsync(auth.Id).Result;
        //             if (deleteModelAddress == null)
        //             {
        //                 finalAuth.Errors.Add("User not found");
        //                 return NotFound(finalAuth);
        //             }
        //             _addressRepo.DeleteAdress(deleteModelAddress);
        //             await _addressRepo.SaveChanges();
        //             return NoContent();
        //         }
        //         finalAuth.Errors.Add("Unauthorized");
        //         return Unauthorized();
        //     }
        //     catch (System.Exception)
        //     {
                
        //         throw;
        //     }
            
        // }
    }
}
