namespace CitizenRegistry.API.Domain.API.Entities
{
    public class Citizen : BaseEntity
    {
        public string? Name { get; set; }
        public string? Cpf { get; set; }
    }
}
