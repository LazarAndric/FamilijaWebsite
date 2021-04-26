using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;
using System.Threading.Tasks;
using FamilijaApi.DTOs;
using FamilijaApi.Models;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AddressesController : ControllerBase
    {
        private IAdddressesRepo _addressRepo;
        private IMapper _mapper;

        public AddressesController (IAdddressesRepo adddressesRepo, IMapper mapper)
        {
            _addressRepo = adddressesRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressReadDto>> GetAddress(int id)
        {
            var content = await _addressRepo.GetAddress(id);
            if (content == null) return NoContent();
            return Ok(_mapper.Map<AddressReadDto>(content));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress ([FromBody] AddressCreateDto addressCreateDto)
        {
            var addres = _mapper.Map<Address>(addressCreateDto);
            _addressRepo.CreateAddress(addres);
            await _addressRepo.SaveChanges();

            return Created("", addres);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress (int id, AddressUpdateDto addressUpdateDto)
        {
            var updateModelAddress = _addressRepo.GetAddress(id).Result;
            if (updateModelAddress == null)
            {
                return NotFound();
            }

            _mapper.Map(addressUpdateDto, updateModelAddress);
            _addressRepo.UpdateAddress(updateModelAddress);
            await _addressRepo.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAddress(int id)
        {
            var deleteModelAddress = _addressRepo.GetAddress(id).Result;
            if (deleteModelAddress == null)
            {
                return NotFound();
            }

            _addressRepo.DeleteUser(deleteModelAddress);
            _addressRepo.SaveChanges();
            return NoContent();
        }
    }
}
