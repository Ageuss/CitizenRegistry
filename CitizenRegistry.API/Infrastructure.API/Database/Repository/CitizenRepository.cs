using CitizenRegistry.API.Domain.API.Entities;
using CitizenRegistry.API.Domain.API.Interfaces;
using CitizenRegistry.API.Infrastructure.API.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace CitizenRegistry.API.Infrastructure.API.Database.Repository
{
    public class CitizenRepository : Repository<Citizen>, ICitizenRepository
    {
        public CitizenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Citizen?> GetCitizenByCpfAsync(string? cpf)
        {
            var citizen =  await _context.Citizens.FirstOrDefaultAsync(c => c.Cpf == cpf);
            return citizen;
        }

        public async Task<IEnumerable<Citizen>> GetCitizenByNameAsync(string? name)
        {
            var query = _context.Citizens.AsQueryable().Where(c => c.Name == name);
            return await query.ToListAsync();
        }
    }
}
