namespace PruebaSisteCredito.Domain.Entities{

    public class AuditLog
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}