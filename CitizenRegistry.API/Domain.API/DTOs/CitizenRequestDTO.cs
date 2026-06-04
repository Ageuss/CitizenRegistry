namespace CitizenRegistry.API.Domain.API.DTOs
{
    public record CitizenRequestDTO
    {
        public string? Name { get; set; }

        private string? _cpf;

        public string? Cpf
        {
            get => _cpf;
            set => _cpf = value?.Replace(".", "").Replace("-", "");
        }
    }
}
