namespace CitizenRegistry.API.Domain.API.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
    }
}
