using AutoMapper;
using FamilijaApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FinancesController : ControllerBase
    {
        private IFinanceRepo _financeRepo;
        private IMapper _mapper;

        public FinancesController(IFinanceRepo financeRepo, IMapper mapper)
        {
            _financeRepo = financeRepo;
            _mapper = mapper;
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<FinanceReadDto>> GetFinance(int id)
        // {
        //     var content = await _financeRepo.GetFinance(id);
        //     if (content == null) return NoContent();
        //     return Ok(_mapper.Map<FinanceReadDto>(content));
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreateFinance([FromBody] FinanceCreateDto finaceCreateDto)
        // {
        //     var finance = _mapper.Map<Finance>(finaceCreateDto);
        //     _financeRepo.CreateFinance(finance);
        //     await _financeRepo.SaveChanges();

        //     return Created("", finance);
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateFinance(int id, FinanceUpdateDto addressUpdateDto)
        // {
        //     var updateModelFinance = await _financeRepo.GetFinance(id);
        //     if (updateModelFinance == null)
        //     {
        //         return NotFound();
        //     }

        //     _mapper.Map(addressUpdateDto, updateModelFinance);
        //     _financeRepo.UpdateFinance(updateModelFinance);
        //     await _financeRepo.SaveChanges();
        //     return Ok();
        // }

        // [HttpDelete("{id}")]
        // public ActionResult DeleteFinance(int id)
        // {
        //     var deleteModelFinance = _financeRepo.GetFinance(id).Result;
        //     if (deleteModelFinance == null)
        //     {
        //         return NotFound();
        //     }

        //     _financeRepo.DeleteUser(deleteModelFinance);
        //     _financeRepo.SaveChanges();
        //     return NoContent();
        // }
    }
}
