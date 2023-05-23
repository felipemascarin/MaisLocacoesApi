using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
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

            RuleFor(rent => rent.Status)
                .Must(status => RentStatus.RentStatusEnum.Contains(status.ToLower()))
                .WithMessage("O Status informado deve existir");

            RuleFor(rent => rent.Carriage)
                .Must(carriage => decimal.TryParse(carriage.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor de frete se inserido deve ser válido e maior que 0")
                .When(rent => rent.Carriage != null);

            //Chama o validator do Address
            RuleFor(rent => rent.Address).SetValidator(new AddressValidator());
        }
    }
}