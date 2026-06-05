using CitizenRegistry.API.Application.API.Services;
using CitizenRegistry.API.Domain.API.Entities;
using CitizenRegistry.API.Domain.API.Interfaces;
using Xunit;

namespace CitizenRegistry.Tests
{
    public class CitizenServiceTests
    {
        private readonly MockCitizenRepository _repository;
        private readonly CitizenService _service;

        public CitizenServiceTests()
        {
            _repository = new MockCitizenRepository();
            _service = new CitizenService(_repository);
        }

        [Fact]
        public async Task CreateCitizenAsync_ShouldSave_WhenCitizenDoesNotExist()
        {
            // Arrange
            var citizen = new Citizen
            {
                Name = "Lucas Oliveira",
                Cpf = "46059954060"
            };

            // Act
            var result = await _service.CreateCitizenAsync(citizen);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Lucas Oliveira", result.Name);
            Assert.Contains(_repository.Citizens, c => c.Cpf == "46059954060");
        }

        [Fact]
        public async Task CreateCitizenAsync_ShouldThrow_WhenCitizenCpfAlreadyExists()
        {
            // Arrange
            var existing = new Citizen
            {
                Id = Guid.NewGuid(),
                Name = "Lucas Oliveira",
                Cpf = "46059954060"
            };
            _repository.Citizens.Add(existing);

            var newCitizen = new Citizen
            {
                Name = "Outro Lucas",
                Cpf = "46059954060" 
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateCitizenAsync(newCitizen)
            );

            Assert.Equal("Cidadão já cadastrado no sistema.", exception.Message);
        }

        [Fact]
        public async Task GetAllCitizensAsync_ShouldReturnAll()
        {
            // Arrange
            _repository.Citizens.Add(new Citizen { Name = "A", Cpf = "1" });
            _repository.Citizens.Add(new Citizen { Name = "B", Cpf = "2" });

            // Act
            var result = await _service.GetAllCitizensAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCitizenByCpfAsync_ShouldReturnCitizen_WhenFound()
        {
            // Arrange
            var target = new Citizen { Name = "Pedro", Cpf = "123" };
            _repository.Citizens.Add(target);

            // Act
            var result = await _service.GetCitizenByCpfAsync("123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pedro", result.Name);
        }

        [Fact]
        public async Task GetCitizenByCpfAsync_ShouldReturnNull_WhenNotFound()
        {
            // Act
            var result = await _service.GetCitizenByCpfAsync("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCitizensByNameAsync_ShouldReturnMatchingCitizens()
        {
            // Arrange
            _repository.Citizens.Add(new Citizen { Name = "Ana Maria", Cpf = "1" });
            _repository.Citizens.Add(new Citizen { Name = "Mariana", Cpf = "2" });
            _repository.Citizens.Add(new Citizen { Name = "Carlos", Cpf = "3" });

            // Act
            var result = await _service.GetCitizensByNameAsync("Mari");

            // Assert
            Assert.Equal(2, result.Count()); 
        }
    }

    public class MockCitizenRepository : ICitizenRepository
    {
        public List<Citizen> Citizens { get; } = new List<Citizen>();

        public Task<Citizen?> GetCitizenByCpfAsync(string? cpf)
        {
            var result = Citizens.FirstOrDefault(c => c.Cpf == cpf);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Citizen>> GetCitizenByNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return Task.FromResult<IEnumerable<Citizen>>(Citizens);

            var result = Citizens.Where(c => c.Name != null && c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }

        public Task<Citizen?> GetByIdAsync(Guid? id)
        {
            var result = Citizens.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Citizen>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Citizen>>(Citizens);
        }

        public Task<Citizen> AddAsync(Citizen entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            Citizens.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<Citizen> UpdateAsync(Citizen entity)
        {
            var existing = Citizens.FirstOrDefault(c => c.Id == entity.Id);
            if (existing != null)
            {
                Citizens.Remove(existing);
                Citizens.Add(entity);
            }
            return Task.FromResult(entity);
        }
    }
}
