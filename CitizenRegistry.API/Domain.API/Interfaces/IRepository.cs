namespace CitizenRegistry.API.Domain.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid? id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
