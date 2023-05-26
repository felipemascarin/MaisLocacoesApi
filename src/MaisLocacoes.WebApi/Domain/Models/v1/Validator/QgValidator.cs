using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class QgValidator : AbstractValidator<QgRequest>
    {
        public QgValidator()
        {
            RuleFor(qg => qg.Description)
                .Must(description => !string.IsNullOrEmpty(description))
                .WithMessage("A Descrição é obrigatória")
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres");

            RuleFor(qg => qg.Latitude)
                .Must(latitude => double.TryParse(latitude.ToString(), out var result))
                .WithMessage("Valor de Latitude se inserido deve ser válido")
                .When(qg => qg.Latitude != null);

            RuleFor(qg => qg.Longitude)
                .Must(longitude => double.TryParse(longitude.ToString(), out var result))
                .WithMessage("Valor de Longitude se inserido deve ser válido ")
                .When(qg => qg.Longitude != null);

            RuleFor(qg => qg.Address).SetValidator(new AddressValidator());
        }
    }
}