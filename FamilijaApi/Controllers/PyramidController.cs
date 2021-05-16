using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.DTOs.Pyramid;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using FamilijaApi.Utility;
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

        public PyramidController(IAuthRepo authRepo, IRoleRepo roleRepo, IUserRepo userRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _jwtTokenUtil = new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        [HttpGet]
        public async Task<IActionResult> GetByReferal(TokenRequest token)
        {
             var auth=await _jwtTokenUtil.VerifyAndGenerateToken(token);
            if (auth.Success)
            {
                var refId = _userRepo.FindReferalAsync(auth.Id);

                var reflist = new List<int>();
                reflist.AddRange((IEnumerable<int>)refId);
                int count = reflist.Count();

                if (count >= 4)
                {
                    return Ok("VIP clan");

                }

                return Ok("Niste vip clan");
            }

            return Unauthorized();
        }

    }


    
}
