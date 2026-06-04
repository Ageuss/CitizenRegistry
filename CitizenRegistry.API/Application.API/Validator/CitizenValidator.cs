using CitizenRegistry.API.Domain.API.DTOs;
using FluentValidation;

namespace CitizenRegistry.API.Application.API.Validator
{
    public class CreateCitizenValidator : AbstractValidator<CitizenRequestDTO>
    {
        public CreateCitizenValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(150).WithMessage("O nome pode ter no máximo 150 caracteres.");

            RuleFor(c => c.Cpf)
                .Cascade(CascadeMode.Stop)
                .Matches("^[0-9]+$").WithMessage("O CPF deve conter apenas números.")
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Length(11).WithMessage("O CPF deve conter exatamente 11 dígitos numéricos.")
                .Must(BeAValidCpf).WithMessage("O CPF informado é inválido.");
        }

        private bool BeAValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (var i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();

            tempCpf += digito;
            soma = 0;

            for (var i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}