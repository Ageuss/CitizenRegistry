using CitizenRegistry.API.Application.API.Validator;
using CitizenRegistry.API.Domain.API.DTOs;
using Xunit;

namespace CitizenRegistry.Tests
{
    public class CitizenValidatorTests
    {
        private readonly CreateCitizenValidator _validator;

        public CitizenValidatorTests()
        {
            _validator = new CreateCitizenValidator();
        }

        [Fact]
        public void Validate_ShouldBeValid_WhenNameAndCpfAreCorrect()
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "José da Silva",
                Cpf = "12345678909" 
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_ShouldFail_WhenNameIsEmptyOrWhitespace(string? name)
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = name,
                Cpf = "12345678909"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_ShouldFail_WhenNameExceeds150Characters()
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = new string('A', 151),
                Cpf = "12345678909"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "O nome pode ter no máximo 150 caracteres.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_ShouldFail_WhenCpfIsEmptyOrWhitespace(string? cpf)
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "Maria Souza",
                Cpf = cpf
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf");
        }

        [Theory]
        [InlineData("1234567890")] // 10 dígitos
        [InlineData("123456789012")] // 12 dígitos
        public void Validate_ShouldFail_WhenCpfHasInvalidLength(string cpf)
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "Maria Souza",
                Cpf = cpf
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "O CPF deve conter exatamente 11 dígitos numéricos.");
        }

        [Fact]
        public void Validate_ShouldFail_WhenCpfHasNonNumericCharacters()
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "Maria Souza",
                Cpf = "1234567890A" // Letra 'A' no final
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "O CPF deve conter apenas números.");
        }

        [Theory]
        [InlineData("11111111111")]
        [InlineData("99999999999")]
        public void Validate_ShouldFail_WhenCpfHasAllIdenticalDigits(string cpf)
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "Maria Souza",
                Cpf = cpf
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "O CPF informado é inválido.");
        }

        [Fact]
        public void Validate_ShouldFail_WhenCpfIsInvalid()
        {
            // Arrange
            var request = new CitizenRequestDTO
            {
                Name = "Maria Souza",
                Cpf = "12345678900" 
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "O CPF informado é inválido.");
        }
    }
}
