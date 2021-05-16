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
        public async Task<IActionResult> GetById([FromBody]TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            if(auth.Success){
                var response = await _userRepo.GetUserByIdAsync(auth.Id);
                if (response == null) return NoContent();
                var finalAuth= _mapper.Map<AuthResult>(auth);
                var model= new CommunicationModel<UserReadDto>(){
                    AuthResult=finalAuth,
                    GenericModel=_mapper.Map<UserReadDto>(response)
                };
                return Ok(model);
            }
            return Unauthorized();
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
        public async Task<IActionResult> UpdateUserAsync([FromBody] GetCommunicationModel<UserUpdateDto> data)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(data.TokenRequest);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var updateModelUser = _userRepo.GetUserByIdAsync(auth.Id).Result;
                if (updateModelUser == null)
                {
                    return NotFound(finalAuth);
                }
                _mapper.Map(data.GenericModel, updateModelUser);
                _userRepo.UpdateUser(updateModelUser);
                await _userRepo.SaveChangesAsync();
                return Ok(finalAuth);
            }
            return Unauthorized(finalAuth);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromBody] TokenRequest token)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
                var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var deleteModelUser = await _userRepo.GetUserByIdAsync(auth.Id);
                if (deleteModelUser == null)
                {
                    return NotFound(finalAuth);
                }
                _userRepo.DeleteUser(deleteModelUser);
                await _userRepo.SaveChangesAsync();
                return Ok(finalAuth);
            }
            return Unauthorized(finalAuth);
        }

        [HttpPut("mail/")]
        public async Task<IActionResult> SendMail([FromBody] Email mail)
        {
          await MailUtility.SendEmailAsyn(mail);
            return Ok();
        }

        [HttpPut("mail/{link}")]
        public async Task<IActionResult> SendConfirmedMail([FromBody] TokenRequest token, string link)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var mail = new Email();
                mail.To = new List<string>();
                var e = await _userRepo.GetUserByIdAsync(auth.Id);
                string email = e.EMail.ToString();
                mail.To.Add(email);
                mail.Subject = "Confirmed Mail";
                mail.Body = " <a href='"+link+"'> Click here </a>";
                await MailUtility.SendEmailAsyn(mail);
                return Ok(finalAuth);
            }
            return Unauthorized(finalAuth);
            
        }

        [HttpPut("mail/confirm")]
        public async Task<IActionResult> UpdateConfirmedMail([FromBody] GetCommunicationModel<ConfirmedEmailDto> datas)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(datas.TokenRequest);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                var updateModelUser = await _userRepo.GetUserByIdAsync(auth.Id);
                if (updateModelUser == null)
                {
                    return NotFound(finalAuth);
                }

                if (updateModelUser.EmailConfirmed == true)
                {
                    return Ok(finalAuth);
                }

                _mapper.Map(datas.GenericModel, updateModelUser);
                _userRepo.UpdateCUser(updateModelUser.EmailConfirmed = true);
                await _userRepo.SaveChangesAsync();
                return Ok(finalAuth);
            }
            return Unauthorized();
        }
    }
}