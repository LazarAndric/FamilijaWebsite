using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.Data.Pyramid;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Pyramid;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Net.Extensions;
using System;
using System.Collections;
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
            _pyramidRepo = pyramidRepo;
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
                var auth = await _jwtTokenUtil.VerifyJwtToken(authorization);
                if (auth.Success)
                {
                    var refId = await _userRepo.FindReferalbyIdAsync(auth.User.Id);

                    if (refId == null)
                    {
                        throw new Exception("User with that RefferalId is not found");
                    }

                    var sortref = from r in refId orderby r.DateRegistration where r.Vip = true select r;


                    if (sortref.Count() >= 4)
                    {
                        var users = _userRepo.GetUserbyReferalId(auth.User.Id);
                        var lusers = new List<User>();
                        lusers.AddRange(users);

                        lusers.ForEach(x =>
                        {
                            x.Qualified = true;
                        });
                        await _userRepo.SaveChangesAsync();
                        throw new Exception("Kvalifikovani Clan");
                    }

                    throw new Exception("Niste kvalifikovani clan");
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateTotalSpent([FromHeader] string authorization)
        {
            try
            {
                var auth = await _jwtTokenUtil.VerifyJwtToken(authorization);
                if (auth.Success)
                {

                    if (auth.User.Qualified == true)
                    {
                        var refId = await _userRepo.FindReferalbyIdAsync(auth.User.Id);

                        if (refId == null)
                        {
                            throw new Exception("User with that RefferalId is not found");
                        }

                        var sortref = from r in refId orderby r.DateRegistration select r;

                        //mi
                        var myref = sortref.Take(1).ToList();
                        myref.AddRange(sortref.Skip(2).Take(1));
                        var idList = new List<int>();
                        foreach (var item in myref)
                        {
                            idList.Add(item.Id);
                        }


                        foreach (var item in idList)
                        {

                            var finMinus = _pyramidRepo.GetFinanceAsync(item);
                            var lfinMinus = new List<Finance>();
                            lfinMinus.AddRange(finMinus);
                            lfinMinus.ForEach(x =>
                            {
                                x.TotalSpent -= 20;

                            });

                            UserTvCreateDto userTvCerateDto = new UserTvCreateDto() { TvId = 1, UserId = item };
                            var usertv = _mapper.Map<UserTv>(userTvCerateDto);
                            await _pyramidRepo.CreateUserTvAsync(usertv);

                            UserTvCreateDto userTvCerateDto2 = new UserTvCreateDto() { TvId = 2, UserId = item };
                            var usertv2 = _mapper.Map<UserTv>(userTvCerateDto2);
                            await _pyramidRepo.CreateUserTvAsync(usertv2);

                            var finPlus = _pyramidRepo.GetFinanceAsync(auth.User.Id);
                            var lfinPlus = new List<Finance>();
                            lfinPlus.AddRange(finPlus);

                            lfinPlus.ForEach(x =>
                            {
                                x.TotalSpent += 20;
                            });

                            await _pyramidRepo.SaveChangesAsync();

                        }

                        //sponsor
                        var sponsorref = sortref.Skip(1).Take(1).ToList();
                        sponsorref.AddRange(sortref.Skip(3).Take(1));
                        var idSList = new List<int>();
                        foreach (var item in sponsorref)
                        {
                            idSList.Add(item.Id);
                        }

                        foreach (var item in idSList)
                        {

                            var finMinus = _pyramidRepo.GetFinanceAsync(item);
                            var lfinMinus = new List<Finance>();
                            lfinMinus.AddRange(finMinus);
                            lfinMinus.ForEach(x =>
                            {
                                x.TotalSpent -= 20;
                            });

                            UserTvCreateDto userTvCerateDto = new UserTvCreateDto() { TvId = 1, UserId = item };
                            var usertv = _mapper.Map<UserTv>(userTvCerateDto);
                            await _pyramidRepo.CreateUserTvAsync(usertv);

                            UserTvCreateDto userTvCerateDto2 = new UserTvCreateDto() { TvId = 3, UserId = item };
                            var usertv2 = _mapper.Map<UserTv>(userTvCerateDto2);
                            await _pyramidRepo.CreateUserTvAsync(usertv2);

                            var finPlus = _pyramidRepo.GetFinanceAsync(auth.User.ReferralId);
                            var lfinPlus = new List<Finance>();
                            lfinPlus.AddRange(finPlus);

                            lfinPlus.ForEach(x =>
                            {
                                x.TotalSpent += 20;
                            });

                            await _pyramidRepo.SaveChangesAsync();
                        }
                        return Ok();
                    }

                    return Ok("You are not eligible for earnings");
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost("/red")]
        public async Task<IActionResult> SetUserTvinRed([FromHeader] string authorization)
        {
            try
            {
                var auth = await _jwtTokenUtil.VerifyJwtToken(authorization);
                if (auth.Success)
                {
                    var novipuser = _userRepo.GetUserbyVipAsync();
                    var idnovipuser = new List<int>();
                    foreach (var item in novipuser)
                    {
                        idnovipuser.Add(item.Id);
                    }

                    foreach (var item in idnovipuser)
                    {
                        var deliteModelUserTv = _pyramidRepo.GetUserTvAsync(item);
                        UserTv userTv = deliteModelUserTv.First();
                        _pyramidRepo.DeleteUserTv(userTv);
                        await _pyramidRepo.SaveChangesAsync();
                    }

                    foreach (var item in idnovipuser)
                    {
                        UserTvCreateDto userTvCerateDto = new UserTvCreateDto() { TvId = 4, UserId = item };
                        var usertv = _mapper.Map<UserTv>(userTvCerateDto);
                        await _pyramidRepo.CreateUserTvAsync(usertv);
                        await _pyramidRepo.SaveChangesAsync();
                    }
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
