using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateRentValidator : AbstractValidator<UpdateRentRequest>
    {
        public UpdateRentValidator()
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

            RuleFor(rent => rent.Address).SetValidator(new UpdateAddressValidator());
        }
    }
}