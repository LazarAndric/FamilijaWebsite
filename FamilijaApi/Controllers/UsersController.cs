using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FamilijaApi.Data;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;
        private JwtTokenUtility _jwtTokenUtil;
        public UsersController(IAuthRepo authRepo, IRoleRepo roleRepo, IUserRepo userRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _jwtTokenUtil=new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        //FOR ADMIN OPTION
        //public async Task<IActionResult> GeAlltUsers()
        //{
        //    var items = await _userRepo.GetAllItems();
        //    if (items == null) return NoContent();
        //    var userlist = new List<User>();
        //    return Ok(_mapper.Map<List<UserReadDto>>(items));
        //}

        [HttpGet]
        public async Task<IActionResult> GetById([FromHeader]string authorization)
        {
            try
            {
                var auth= await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){
                    var response = await _userRepo.GetUserByIdAsync(auth.User.Id);
                    if (response == null) 
                        throw new System.Exception("User not found");
                    return Ok(_mapper.Map<UserReadDto>(response));
                }
                throw new System.Exception("User is not authorize");
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

        // [HttpPost]
        // public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        // {
        //     var user = _mapper.Map<User>(userCreateDto);
        //     await _userRepo.CreateUserAsync(user);
        //     await _userRepo.SaveChanges();

        //     return Created("", user);
        // }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromHeader]string authorization, UserCreateDto user)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(auth.Success){
                    var updateModelUser = await _userRepo.GetUserByIdAsync(auth.User.Id);
                    if (updateModelUser == null)
                    {
                        return NotFound(new AuthResult(){
                            Success=false,
                            Errors=new List<string>(){
                                "User not found"
                            }
                        });
                    }
                    _mapper.Map(user, updateModelUser);
                    _userRepo.UpdateUser(updateModelUser);
                    await _userRepo.SaveChangesAsync();
                    return Ok(new AuthResult(){
                        Success=true
                    });
                }
                throw new System.Exception();
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
        // public async Task<IActionResult> DeleteUserAsync([FromHeader] string authorization)
        // {
        //     try
        //     {
        //         var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
        //         if(auth.Success){
        //             var deleteModelUser = await _userRepo.GetUserByIdAsync(auth.User.Id);
        //             if (deleteModelUser == null)
        //             {
        //                 return NotFound(new AuthResult(){
        //                     Success=false,
        //                     Errors=new List<string>(){
        //                         "User not found"
        //                     }
        //                 });
        //             }
        //             _userRepo.DeleteUser(deleteModelUser);
        //             await _userRepo.SaveChangesAsync();
        //             return RedirectToAction("logOut","AuthorizationsController");
        //         }
        //         return Unauthorized();
        //     }
        //     catch (System.Exception)
        //     {
                
        //         throw;
        //     }
        // }

        [HttpPut("mail/")]
        public async Task<IActionResult> SendMail([FromBody] Email mail)
        {
          await MailUtility.SendEmailAsyn(mail);
            return Ok();
        }

        [HttpPut("mail/{link}")]
        public async Task<IActionResult> SendConfirmedMail([FromHeader] string authorization, string link)
        {
            var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var mail = new Email();
                mail.To = new List<string>();
                string email = auth.User.EMail.ToString();
                mail.To.Add(email);
                mail.Subject = "Confirmed Mail";
                mail.Body = " <a href='"+link+"'> Click here </a>";
                await MailUtility.SendEmailAsyn(mail);
                return Ok(finalAuth);
            }
            return Unauthorized();
            
        }

        [HttpPut("mail/confirm")]
        public async Task<IActionResult> UpdateConfirmedMail([FromHeader] string authorization, ConfirmedEmailDto confirmedMail)
        {
            var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
            if(auth.Success){

                if (auth.User.EmailConfirmed == true)
                {
                    return Ok();
                }

                _mapper.Map(confirmedMail, auth.User);
                _userRepo.UpdateCUser(auth.User.EmailConfirmed = true);
                await _userRepo.SaveChangesAsync();
                return Ok();
            }
            return Unauthorized();
        }
    }
}