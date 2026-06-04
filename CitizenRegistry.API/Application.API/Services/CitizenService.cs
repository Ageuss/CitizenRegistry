using CitizenRegistry.API.Domain.API.Entities;
using CitizenRegistry.API.Domain.API.Interfaces;

namespace CitizenRegistry.API.Application.API.Services
{
    public class CitizenService : ICitizenService
    {
        private ICitizenRepository _repository;
        public CitizenService(ICitizenRepository repository) 
        { 
            _repository = repository;
        }

        public async Task<Citizen> CreateCitizenAsync(Citizen citizen)
        {
            var existingCitizen = await _repository.GetCitizenByCpfAsync(citizen.Cpf);

            if (existingCitizen != null)
                throw new InvalidOperationException("Cidadão já cadastrado no sistema.");

            return await _repository.AddAsync(citizen);
        }


        public async Task<Citizen?> GetCitizenByIdAsync(Guid? id)
        {
            var citizen = await _repository.GetByIdAsync(id);

            return citizen;
        }

        public async Task<IEnumerable<Citizen>> GetAllCitizensAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Citizen>> GetCitizensByNameAsync(string? name)
        {
            return await _repository.GetCitizenByNameAsync(name);
        }

        public async Task<Citizen?> GetCitizenByCpfAsync(string? cpf)
        {
            return await _repository.GetCitizenByCpfAsync(cpf);
        }

        public Task DeleteCitizenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCitizenAsync(Citizen citizen)
        {
            throw new NotImplementedException();
        }
    }
}
