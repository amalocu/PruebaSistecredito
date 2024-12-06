namespace PruebaSisteCredito.Domain.Entities {
public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int AreaId { get; set; }
        public int? LeaderId { get; set; }
        public bool IsManager { get; set; }

    }
}