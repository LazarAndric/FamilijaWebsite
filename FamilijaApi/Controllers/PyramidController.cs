using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Pyramid;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class PyramidController : ControllerBase
    {
        private JwtTokenUtility _jwtTokenUtil;
        private IUserRepo _userRepo;
        private IMapper _mapper;
        private IFinanceRepo _financeRepo;

        public PyramidController(IAuthRepo authRepo, IRoleRepo roleRepo, IUserRepo userRepo, IMapper mapper, IFinanceRepo financeRepo, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _financeRepo = financeRepo;
            _jwtTokenUtil = new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        [HttpGet]
        public async Task<IActionResult> GetByReferal([FromHeader] string authorization)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorization);
                if (auth.Success)
                {
                    var refId = await _userRepo.FindReferalbyIdAsync(auth.User.Id);

                    if (refId == null)
                    {
                        throw new Exception("User with that RefferalId is not found");
                    }

                    var sortref = from r in refId orderby r.DateRegistration select r;
                   

                    if (refId.Count() >= 4)
                    {
                        throw new Exception("Vip Clan");
                    }

                   

                    throw new Exception("Niste vip clan");
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> GetReferalByDate([FromHeader] string authorization)
        {
            try
            {
                var auth = await _jwtTokenUtil.VerifyJwtToken(authorization);
                if (auth.Success)
                {
                    var refId = await _userRepo.FindReferalbyIdAsync(auth.User.Id);

                    if (refId == null)
                    {
                        throw new Exception("User with that RefferalId is not found");
                    }

                    var sortref = from r in refId orderby r.DateRegistration select r;
                    var myref = sortref.Take(1).ToList();
                    myref.AddRange(sortref.Skip(2).Take(1));
                    //myref.AddRange(sortref.Skip(4));
                    
                    //_mapper.Map(financeUpdateDto, );
                    //_financeRepo.UpdateFinance(auth.);
                    //await _financeRepo.SaveChanges();









                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }

        }

    }


    
}
