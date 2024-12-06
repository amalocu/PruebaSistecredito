using Microsoft.AspNetCore.Mvc;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.AspNetCore.Authorization;
using PruebaSisteCredito.Application.Logic;

namespace PruebaSisteCredito.Application.Controllers{


    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAreaRepository _areaRepository;

        public FileController(IEmployeeRepository employeeRepository, IAreaRepository areaRepository)
        {
            _employeeRepository = employeeRepository;
            _areaRepository = areaRepository;
        }

       [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Procesar el archivo Excel
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                using var package = new OfficeOpenXml.ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0]; // Primera hoja del Excel
                var rowCount = worksheet.Dimension.Rows;

                var employees = new List<Employee>();

                // Leer los datos del Excel (ignorando la primera fila de encabezados)
                for (int row = 2; row <= rowCount; row++)
                {

                    var area = new Area {
                        Id = Int32.Parse(worksheet.Cells[row, 3].Text),
                        Name = worksheet.Cells[row, 4].Text,
                        AdminId = Int32.Parse(worksheet.Cells[row, 9].Text),
                    };
                    await SaveArea(area);
                    
                    var employee = new Employee
                    {
                        Id = Int32.Parse(worksheet.Cells[row, 1].Text),
                        Name = worksheet.Cells[row, 2].Text,
                        AreaId = area.Id,
                        Position = worksheet.Cells[row, 5].Text,
                        IsManager = worksheet.Cells[row, 6].Text != "No",
                        LeaderId =  worksheet.Cells[row, 7].Text == ""? null: Int32.Parse(worksheet.Cells[row, 7].Text),
                    };
                    await SaveEmployee(employee);
                    employees.Add(employee);
                }
                return Ok(new { Message = "Datos cargados.", Employees = employees.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private async Task SaveEmployee(Employee employee)
        {
            //Validar regla de negocio
            EmployeeLogic.RulerValidator(employee);
            employee.Id = 0;
            await _employeeRepository.AddAsync(employee) ;
        }

        private async Task SaveArea(Area area)
        {
            //Validar si Existe
            Area a = await _areaRepository.GetByIdAsync(area.Id) ;
            //Si no existe crear registro
            if (a == null){
                area.Id=0;
               await _areaRepository.AddAsync(area);
            }

        }
    }
}