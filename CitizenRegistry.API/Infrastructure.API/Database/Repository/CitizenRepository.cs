using CitizenRegistry.API.Domain.API.Entities;
using CitizenRegistry.API.Domain.API.Interfaces;
using CitizenRegistry.API.Infrastructure.API.Database.Context;

namespace CitizenRegistry.API.Infrastructure.API.Database.Repository
{
    public class CitizenRepository : Repository<Citizen>, ICitizenRepository
    {
        public CitizenRepository(AppDbContext context) : base(context)
        {
        }
    }
}
