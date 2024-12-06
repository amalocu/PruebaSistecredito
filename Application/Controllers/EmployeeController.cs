using Microsoft.AspNetCore.Mvc;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.AspNetCore.Authorization;
using PruebaSisteCredito.Application.Logic;

namespace PruebaSisteCredito.Application.Controllers{


    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IAreaRepository _areaRepository;

        public EmployeeController(IEmployeeRepository repository, IAreaRepository areaRepository)
        {
            _repository = repository;
            _areaRepository = areaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _repository.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee is not null ? Ok(employee) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            await EmployeeLogic.Validate(employee, _repository, _areaRepository);
            await _repository.AddAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();
            await EmployeeLogic.Validate(employee, _repository, _areaRepository);
            await _repository.UpdateAsync(employee);
            return Ok(employee) ;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok($"Registro {id} eliminado con exito."); 
        }
    }
}