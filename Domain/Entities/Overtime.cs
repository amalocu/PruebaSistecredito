namespace PruebaSisteCredito.Domain.Entities {
public class OverTime
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate {get; set;} 
        public DateTime FinishDate {get; set;} 
        public decimal HoursWorked {get; set;} 
        public String? Reason {get; set;} 
        public String? Status {get; set;} 
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public int? UserId { get; set; }
    }
}