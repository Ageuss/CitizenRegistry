using CitizenRegistry.API.Domain.API.Entities;

namespace CitizenRegistry.API.Domain.API.Interfaces
{
    public interface ICitizenRepository : IRepository<Citizen>
    {
        Task<Citizen?> GetCitizenByCpfAsync(string? cpf);
        Task<IEnumerable<Citizen>> GetCitizenByNameAsync(string? name);
    }
}
