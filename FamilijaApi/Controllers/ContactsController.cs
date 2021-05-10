using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using FamilijaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ContactsController : ControllerBase
    {
        private IContactRepo _contactRepo;
        private IMapper _mapper;

        public ContactsController (IContactRepo contactRepo, IMapper mapper)
        {
            _contactRepo = contactRepo;
            _mapper = mapper;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ContactReadDto>> GetContact(int id)
        //{
        //    var content = await _contactRepo.GetContact(id);
        //    if (content == null) return NoContent();
        //    return Ok(_mapper.Map<ContactReadDto>(content));
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateContact([FromBody] ContactCreateDto contactCreateDto)
        //{
        //    var contact = _mapper.Map<Contact>(contactCreateDto);
        //    _contactRepo.CreateContact(contact);
        //    await _contactRepo.SaveChanges();

        //    return Created("", contact);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateContact(int id, ContactUpdateDto contactUpdateDto)
        //{
        //    var updateModelContact = _contactRepo.GetContact(id).Result;
        //    if (updateModelContact == null)
        //    {
        //        return NotFound();
        //    }

        //    _mapper.Map(contactUpdateDto, updateModelContact);
        //    _contactRepo.UpdateContact(updateModelContact);
        //    await _contactRepo.SaveChanges();
        //    return Ok();

        //}

        //[HttpDelete("{id}")]
        //public ActionResult DeleteContact(int id)
        //{
        //    var deleteModelContact = _contactRepo.GetContact(id).Result;
        //    if (deleteModelContact == null)
        //    {
        //        return NotFound();
        //    }

        //    _contactRepo.DeleteContact(deleteModelContact);
        //    _contactRepo.SaveChanges();
        //    return NoContent();
        //}

    }
}
