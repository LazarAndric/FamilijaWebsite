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
<<<<<<< Updated upstream

        public PyramidController(IAuthRepo authRepo, IRoleRepo roleRepo, IUserRepo userRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _mapper = mapper;
            _userRepo = userRepo;
=======
        private IFinanceRepo _financeRepo;
        private FamilijaDbContext _context;
        public PyramidController(IAuthRepo authRepo, IRoleRepo roleRepo, IUserRepo userRepo, IMapper mapper, IFinanceRepo financeRepo, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation, FamilijaDbContext familijaDbContext)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _financeRepo = financeRepo;
            _context = familijaDbContext;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
                    if (refId.Count() >= 4)
=======
                    var sortref = from r in refId orderby r.DateRegistration where r.Vip = true select r;
                   

                    if (sortref.Count() >= 4)
>>>>>>> Stashed changes
                    {
                        var finMinus = _context.Users.Where(x => x.Id == auth.User.Id).ToList();
                        finMinus.ForEach(x =>
                        {
                            x.Qualified = true;
                        });
                        _context.SaveChanges();
                        throw new Exception("Kvalifikovani Clan");
                    }
<<<<<<< Updated upstream
                    
                    throw new Exception("Niste vip clan");
=======

                   

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

                        var sortref = from r in refId orderby r.DateRegistration  select r;


                        var myref = sortref.Take(1).ToList();
                        myref.AddRange(sortref.Skip(2).Take(1));
                        var idList = new List<int>();
                        foreach (var item in myref)
                        {
                            idList.Add(item.Id);
                        }


                        foreach (var item in idList)
                        {

                            var finMinus = _context.Finance.Where(x => x.UserId == item).ToList();
                            finMinus.ForEach(x =>
                            {
                                x.TotalSpent -= 20;
                                
                            });
                            
                            var finPlus = _context.Finance.Where(x => x.UserId == auth.User.Id).ToList();
                            finPlus.ForEach(x =>
                            {
                                x.TotalSpent += 20;
                            });

                            _context.SaveChanges();

                        }
                        var sponsorref = sortref.Skip(1).Take(1).ToList();
                        sponsorref.AddRange(sortref.Skip(3).Take(1));
                        var idSList = new List<int>();
                        foreach (var item in sponsorref)
                        {
                            idSList.Add(item.Id);
                        }

                        foreach (var item in idSList)
                        {

                            var finMinus = _context.Finance.Where(x => x.UserId == item).ToList();
                            finMinus.ForEach(x =>
                            {
                                x.TotalSpent -= 20;
                            });
                           
                            var finPlus = _context.Finance.Where(x => x.UserId == auth.User.ReferralId).ToList();
                            finPlus.ForEach(x =>
                            {
                                x.TotalSpent += 20;
                            });

                            _context.SaveChanges();

                        }

                        return Ok();
                    }

                    return Ok("You are not eligible for earnings");
>>>>>>> Stashed changes
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
