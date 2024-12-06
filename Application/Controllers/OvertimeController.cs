using Microsoft.AspNetCore.Mvc;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.AspNetCore.Authorization;
using PruebaSisteCredito.Application.Logic;

namespace PruebaSisteCredito.Application.Controllers{


    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OverTimeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IOverTimeRepository _repository;

        public OverTimeController(IEmployeeRepository employeeRepository, IAreaRepository areaRepository, IOverTimeRepository repository)
        {
            _employeeRepository = employeeRepository;
            _areaRepository = areaRepository;
            _repository = repository;
        }

       [HttpPost("report")]
        public async Task<IActionResult> Report(OverTime overTime)
        {
            try
            {
                overTime.HoursWorked = OverTimeLogic.GetTotalHours(overTime.StartDate, overTime.FinishDate);
                await OverTimeLogic.ValidateHours(overTime, _repository);
                await _repository.AddOverTime(overTime);
                return Ok(new { Message = "Datos registrados." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{employeeid}/{startdate}/{enddate}")]
        public async Task<IActionResult> GetOverTimeByEmployee(int employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {

                var overtimes = await _repository.GetOverTimeByEmployeeAndDateRange( employeeId,  startDate,  endDate);
                return Ok(overtimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("approval")]
        public async Task<IActionResult> Approval(OverTime overTime)
        {
            try
            {
                var tipo = 0;
                //Obtener datos de empleado
                Employee employee = await _employeeRepository.GetByIdAsync(overTime.EmployeeId) ?? throw new Exception($"Empleado {overTime.EmployeeId} no existe");
                //Obtener datos de area.
                Area area = await _areaRepository.GetByIdAsync(employee.AreaId)??throw new Exception($"Area {employee.AreaId} no existe");
                //Identificar si es gerente, RH o Lider.
                //Si es Lider validar que sea lider del empleado.
                if(employee.LeaderId==overTime.UserId){
                    tipo = 3; //Lider de Area
                }
                //Si es RH Validar que tenga asignada la area del empleado.
                if(area.AdminId == overTime.UserId){
                    tipo = 2; //Recursos Humanos asignado
                } 
                Employee manager = await _employeeRepository.GetByIdAsync(overTime.UserId??0);
                if(manager!=null && manager.IsManager){
                    tipo = 1;
                }
                if(tipo==0){
                    throw new Exception($"Empleado {overTime.EmployeeId} no puede aprobar horas extras.");
                }

                //Si es Lider Validar que ya no este aprobado o Rechazadio. por RH o Gerente.
                //Si es RH Validar que ya no este aprobado o Rechazado por Gerente.
                OverTime previus = await _repository.GetByIdAsync(overTime.Id)??throw new Exception($"Registro {overTime.Id} no existe");             
                if( (tipo == 3 && ( (manager!=null && previus.UserId == manager.Id) || previus.UserId == area.AdminId)) ||  (tipo == 2 && manager!=null && previus.UserId == manager.Id  ) ){
                   throw new Exception($"Registro {overTime.Id} ya se encuentra {previus.Status} por {previus.UserId}."); 
                }
                await _repository.UpdateAsync(overTime);
                return Ok(new { Message = "Datos actualizados." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message} {ex.StackTrace}");
            }
        }


        
    }
}