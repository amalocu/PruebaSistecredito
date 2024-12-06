using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Impl;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;

namespace PruebaSisteCredito.Application.Logic
{
    public class EmployeeLogic
    {
        internal static void RulerValidator(Employee employee)
        {
            if( (employee.LeaderId == 0 || employee.LeaderId == null) && !employee.IsManager ){
                throw new Exception($"{employee.Name} no tiene lider asociado.");
            }
        }

        internal static async Task Validate(Employee employee, IEmployeeRepository _repository, IAreaRepository _areaRepository)
        {
            //Validar si el area existe
            Area area = await _areaRepository.GetByIdAsync(employee.AreaId) ?? throw new Exception($"Area {employee.AreaId} no existe.");
            //Validar si el Lider existe
            int leaderId = employee.LeaderId??0;
            if(leaderId != 0)
            {
                Employee leader  = await _repository.GetByIdAsync(leaderId) ?? throw new Exception($" Lider {employee.LeaderId} no existe.");
            }
            //Validar regla.
            RulerValidator(employee);
        }
    }
}