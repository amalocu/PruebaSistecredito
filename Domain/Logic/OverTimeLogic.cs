using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Impl;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;

namespace PruebaSisteCredito.Application.Logic
{
    public class OverTimeLogic
    {
        internal static decimal GetTotalHours(DateTime startDate, DateTime finishDate)
        {
            TimeSpan difference = finishDate - startDate; // Calcula la diferencia entre las fechas
            return (decimal)difference.TotalHours; // Total de horas (incluye fracciones)
        }

        internal static async Task ValidateHours(OverTime overTime, IOverTimeRepository repository)
        {
            //Obtener las horas actuales del mes.
            var (firstDay, lastDay) = GetDateRangeForMonth(overTime.StartDate.Year,overTime.StartDate.Month);
            decimal totalHours = await repository.GetSumByUserAndDateRangeAsync(overTime.EmployeeId, firstDay, lastDay) + overTime.HoursWorked; 

            //Validar el total
            if(totalHours > 40){
                throw new Exception($"El total de {totalHours} horas, supera el limite permitido (40) para el periodo {overTime.StartDate.Year}{overTime.StartDate.Month}.");
            }
        }

        public static (DateTime FirstDay, DateTime LastDay) GetDateRangeForMonth(int year, int month)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month)).AddDays(1).AddTicks(-1);
            return (firstDay, lastDay);
        }
    }
}