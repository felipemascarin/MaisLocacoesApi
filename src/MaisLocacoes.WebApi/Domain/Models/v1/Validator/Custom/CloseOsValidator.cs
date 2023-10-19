using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.Custom
{
    public class CloseOsValidator : AbstractValidator<FinishOsRequest>
    {
        public CloseOsValidator()
        {
            RuleFor(closeOs => closeOs.ProductTuitionId)
                .Must(productTuitionId => int.TryParse(productTuitionId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id do ProductTuition deve ser um número inteiro maior que 0");

            RuleFor(closeOs => closeOs.ProductCode)
                .MaximumLength(255)
                .WithMessage("O Código do produto ultrapassou o limite máximo de caracteres")
               .When(closeOs => !string.IsNullOrEmpty(closeOs.ProductCode));

            RuleFor(closeOs => closeOs.QgId)
                .Must(qgId => int.TryParse(qgId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id do QG se inserido deve ser um número inteiro maior que 0")
                .When(closeOs => closeOs.QgId != null);

            RuleFor(closeOs => closeOs.DeliveryObservation)
               .MaximumLength(1000)
               .WithMessage("Observação do intregador ultrapassou o limite máximo de caracteres")
               .When(closeOs => !string.IsNullOrEmpty(closeOs.DeliveryObservation));

            RuleFor(rentedPlace => rentedPlace.Latitude)
                .Must(latitude => double.TryParse(latitude.ToString(), out var result))
                .WithMessage("Valor de Latitude se inserido deve ser válido")
                .When(rentedPlace => rentedPlace.Latitude != null);

            RuleFor(rentedPlace => rentedPlace.Longitude)
                .Must(longitude => double.TryParse(longitude.ToString(), out var result))
                .WithMessage("Valor de Longitude se inserido deve ser válido ")
                .When(rentedPlace => rentedPlace.Longitude != null);
        }
    }
}