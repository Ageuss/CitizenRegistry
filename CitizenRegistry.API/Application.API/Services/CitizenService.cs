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

        public async Task CreateCitizenAsync(Citizen citizen)
        {
            try
            {
                await _repository.AddAsync(citizen);
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public async Task<Citizen?> GetCitizenByIdAsync(Guid? id)
        {
           return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Citizen>> GetAllCitizensAsync()
        {
            return await _repository.GetAllAsync();
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
