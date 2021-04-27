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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(int id, PersonalInfoUpdateDtos personalInfoUpdateDtos)
        {
            var updateModelPersonalInfo = _personalInfoRepo.GetPersonalInfo(id).Result;
            if (updateModelPersonalInfo == null)
            {
                return NotFound();
            }

            _mapper.Map(personalInfoUpdateDtos, updateModelPersonalInfo);
            _personalInfoRepo.UpdatePersonalInfo(updateModelPersonalInfo);
            await _personalInfoRepo.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult DeletePersonalInfo(int id)
        {
            var deleteModelPersonalInfo = _personalInfoRepo.GetPersonalInfo(id).Result;
            if (deleteModelPersonalInfo == null)
            {
                return NotFound();
            }

            _personalInfoRepo.DeletePersonalInfo(deleteModelPersonalInfo);
            _personalInfoRepo.SaveChanges();
            return NoContent();
        }
    }
}
