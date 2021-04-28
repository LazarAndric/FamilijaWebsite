using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RolesController : ControllerBase
    {
        private IRoleRepo _roleRepo;
        private IMapper _mapper;

        public RolesController(IRoleRepo roleRepo, IMapper mapper)
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleReadDto>> GetRole(int id)
        {
            var content = await _roleRepo.GetRole(id);
            if (content == null) return NoContent();
            return Ok(_mapper.Map<RoleReadDto>(content));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleCreateDto)
        {
            var role = _mapper.Map<Role>(roleCreateDto);
            _roleRepo.CreateRole(role);
            await _roleRepo.SaveChanges();

            return Created("", role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleUpdateDto roleUpdateDto)
        {
            var updateModelRole = _roleRepo.GetRole(id).Result;
            if (updateModelRole == null)
            {
                return NotFound();
            }

            _mapper.Map(roleUpdateDto, updateModelRole);
            _roleRepo.UpdateRole(updateModelRole);
            await _roleRepo.SaveChanges();
            return Ok();

        }


        [HttpDelete("{id}")]
        public ActionResult DeleteRole(int id)
        {
            var deleteModelRole = _roleRepo.GetRole(id).Result;
            if (deleteModelRole == null)
            {
                return NotFound();
            }

            _roleRepo.DeleteRole(deleteModelRole);
            _roleRepo.SaveChanges();
            return NoContent();
        }
    }
}
