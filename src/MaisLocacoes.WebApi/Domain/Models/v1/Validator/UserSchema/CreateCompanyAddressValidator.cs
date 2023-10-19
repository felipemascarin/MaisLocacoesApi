using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema
{
    public class CreateCompanyAddressValidator : AbstractValidator<CreateCompanyAddressRequest>
    {
        public CreateCompanyAddressValidator()
        {
            RuleFor(companyAddress => companyAddress.Number)
                .Must(number => long.TryParse(number, out var result) && result >= 0)
                .WithMessage("O Número do endereço se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("O Número do endereço ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Number));

            RuleFor(companyAddress => companyAddress.Cep)
                .Matches(@"^\d{5}-\d{3}$")
                .WithMessage("CEP se informado deve ser válido")
                .When(client => !string.IsNullOrEmpty(client.Cep));

            RuleFor(companyAddress => companyAddress.Street)
                .MaximumLength(255)
                .WithMessage("Rua ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Street));

            RuleFor(companyAddress => companyAddress.Complement)
               .MaximumLength(255)
               .WithMessage("Complemento ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.Complement));

            RuleFor(companyAddress => companyAddress.District)
               .MaximumLength(255)
               .WithMessage("Bairro ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.District));

            RuleFor(companyAddress => companyAddress.City)
               .MaximumLength(255)
               .WithMessage("Cidade ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.City));

            RuleFor(companyAddress => companyAddress.State)
               .MaximumLength(255)
               .WithMessage("Estado ultrapassou o limite máximo de caracteres")
               .When(client => !string.IsNullOrEmpty(client.State));

            RuleFor(companyAddress => companyAddress.Country)
              .MaximumLength(255)
              .WithMessage("País ultrapassou o limite máximo de caracteres")
              .When(client => !string.IsNullOrEmpty(client.Country));
        }
    }
}