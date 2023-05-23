using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using System.Text.RegularExpressions;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class AddressValidator : AbstractValidator<AddressRequest>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Number)
                .Must(number => long.TryParse(number, out var result) && result >= 0)
                .WithMessage("O Número do endereço se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("O Número do endereço ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Number));

            RuleFor(address => address.Cep)
                .Matches(@"^\d{5}-\d{3}$")
                .WithMessage("CEP se informado deve ser válido")
                .When(client => !string.IsNullOrEmpty(client.Cep));

            RuleFor(address => address.Street)
                .MaximumLength(255)
                .WithMessage("Rua ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Street));

            RuleFor(address => address.Complement)
               .MaximumLength(255)
               .WithMessage("Complemento ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.Complement));

            RuleFor(address => address.District)
               .MaximumLength(255)
               .WithMessage("Bairro ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.District));

            RuleFor(address => address.City)
               .MaximumLength(255)
               .WithMessage("Cidade ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.City));

            RuleFor(address => address.State)
               .MaximumLength(255)
               .WithMessage("Estado ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.State));

            RuleFor(address => address.Country)
              .MaximumLength(255)
              .WithMessage("País ultrapassou o limite máximo de caracteres")
              .When(client => !string.IsNullOrEmpty(client.Country));
        }
    }
}