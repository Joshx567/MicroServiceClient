namespace ServiceClient.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string FirstLastname { get; set; } = null!;
        public string? SecondLastname { get; set; }
        public DateTime DateBirth { get; set; }
        public string Ci { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string FitnessLevel { get; set; }
        public decimal InitialWeightKg { get; set; }
        public decimal CurrentWeightKg { get; set; }
        public string EmergencyContactPhone { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = null!;           
        public DateTime? LastModification { get; set; }
        public string? LastModifiedBy { get; set; }            
    }

}
