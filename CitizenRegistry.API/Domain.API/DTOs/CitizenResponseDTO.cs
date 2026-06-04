namespace CitizenRegistry.API.Domain.API.DTOs
{
    public record CitizenResponseDTO
    {
        public string? Name { get; set; }
        public string? Cpf { get; set; }
    }
}
