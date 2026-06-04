using CitizenRegistry.API.Domain.API.Entities;

namespace CitizenRegistry.API.Domain.API.Interfaces
{
    public interface ICitizenService
    {
        Task<Citizen?> GetCitizenByIdAsync(Guid? id);
        Task<IEnumerable<Citizen>> GetCitizensByNameAsync(string? name);
        Task<Citizen?> GetCitizenByCpfAsync(string? cpf);
        Task<IEnumerable<Citizen>> GetAllCitizensAsync();
        Task<Citizen> CreateCitizenAsync(Citizen citizen);
        Task DeleteCitizenAsync(Guid id);
        Task UpdateCitizenAsync(Citizen citizen);
    }
}
