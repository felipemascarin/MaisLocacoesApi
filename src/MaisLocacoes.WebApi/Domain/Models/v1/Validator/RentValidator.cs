using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enum;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class RentValidator : AbstractValidator<RentRequest>
    {
        public RentValidator()
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

            RuleFor(rent => rent.FirstDueDate)
               .Must(firstDueDate => DateTime.TryParse(firstDueDate.ToString(), out var result))
               .WithMessage("A Data se inserida deve ser uma data válida")
               .When(rent => rent.FirstDueDate != null);

            RuleFor(rent => rent.Address).SetValidator(new AddressValidator());
        }
    }
}