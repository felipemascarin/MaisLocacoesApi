using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enum;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CreateRentValidator : AbstractValidator<CreateRentRequest>
    {
        public CreateRentValidator()
        {
            RuleFor(rent => rent.ClientId)
               .Must(clientId => int.TryParse(clientId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do cliente deve ser um número inteiro maior que 0");

            RuleFor(rent => rent.Carriage)
                .Must(carriage => decimal.TryParse(carriage.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor de frete se inserido deve ser válido")
                .When(rent => rent.Carriage != null);

            RuleFor(rent => rent.Description)
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres")
                .When(rent => !string.IsNullOrEmpty(rent.Description));

            RuleFor(rent => rent.UrlSignature)
                .MaximumLength(2048)
                .WithMessage("UrlSignature ultrapassou o limite máximo de caracteres")
                .When(rent => !string.IsNullOrEmpty(rent.UrlSignature));

            RuleFor(rent => rent.Address).SetValidator(new CreateAddressValidator());
        }
    }
}