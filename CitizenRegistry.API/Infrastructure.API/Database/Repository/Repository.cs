using CitizenRegistry.API.Domain.API.Interfaces;
using CitizenRegistry.API.Infrastructure.API.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace CitizenRegistry.API.Infrastructure.API.Database.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(AppDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(Guid? id)
        {
            return await _set.FindAsync(id).AsTask();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        async Task<T> IRepository<T>.AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
