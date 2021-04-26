using Microsoft.AspNetCore.Mvc;
using FamilijaApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FailijaApi.Data;
using AutoMapper;
using FamilijaApi.Models;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PersonalInfoController : ControllerBase
    {
        private IPersonalInfoRepo _personalInfoRepo;
        private IMapper _mapper;

        public PersonalInfoController(IPersonalInfoRepo personalInfoRepo, IMapper mapper)
        {
            _personalInfoRepo = personalInfoRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalInfoReadDto>> GetPersonalInfo(int id)
        {
            var content = await _personalInfoRepo.GetPersonalInfo(id);
            if (content == null) return NoContent();
            return Ok(_mapper.Map<PersonalInfoReadDto>(content));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonalInfo([FromBody] PersonalInfoCreateDto personalInfoCreateDto)
        {
            var personalInfo = _mapper.Map<PersonalInfo>(personalInfoCreateDto);
            _personalInfoRepo.CreatePersonalInfo(personalInfo);
            await _personalInfoRepo.SaveChanges();

            return Created("", personalInfo);
        }
    }
}
