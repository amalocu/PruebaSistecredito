using Microsoft.AspNetCore.Mvc;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.AspNetCore.Authorization;

namespace PruebaSisteCredito.Application.Controllers{


    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _repository;

        public AreaController( IAreaRepository areaRepository)
        {
            _repository = areaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _repository.GetAllAsync();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var area = await _repository.GetByIdAsync(id);
            return area is not null ? Ok(area) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Area area)
        {
            await _repository.AddAsync(area);
            return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Area area)
        {
            if (id != area.Id) return BadRequest();
            await _repository.UpdateAsync(area);
            return Ok(area) ;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok($"Registro {id} eliminado con exito."); 
        }
    }
}