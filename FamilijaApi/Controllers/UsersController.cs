using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController : ControllerBase
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;

        public UsersController(IUserRepo userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo=userRepo;
        }

        // public async Task<IActionResult> GeAlltUsers()
        // {
            // var items=await _userRepo.GetAllItems();
            // if(items==null)     return NoContent();
            // return Ok(_mapper.Map<List<UserReadDto>>(items));

        // }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(int id){
            // var response=await _userRepo.GetUserById(id);
                // if(response==null)      return NoContent();
            // return Ok(_mapper.Map<UserReadDto>(response));
        // }
    }
}