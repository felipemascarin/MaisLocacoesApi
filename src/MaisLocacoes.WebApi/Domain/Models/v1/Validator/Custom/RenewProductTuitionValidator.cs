using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.Custom
{
    public class RenewProductTuitionValidator : AbstractValidator<RenewProductTuitionRequest>
    {
        public RenewProductTuitionValidator()
        {
            RuleFor(productTuition => productTuition.Id)
               .Must(id => int.TryParse(id.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id da locação deve ser um número inteiro maior que 0");

            RuleFor(product => product.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("O Valor de locação do produto inserido não é válido");

            RuleFor(product => product.InitialDateTime)
                 .Must(initialDateTime => DateTime.TryParse(initialDateTime.ToString(), out var result))
                 .WithMessage("A Data de início da locação deve ser uma data válida");

            RuleFor(product => product.FinalDateTime)
                 .Must(finalDateTime => DateTime.TryParse(finalDateTime.ToString(), out var result))
                 .WithMessage("A Data de entrega da locação deve ser uma data válida");

            RuleFor(product => product.FinalDateTime)
                .Null()
                .WithMessage("A Data de entrega deve ser maior que a Data de início")
                .When(product => product.FinalDateTime < product.InitialDateTime);

            RuleFor(product => product.FirstDueDate)
                .Must(firstDueDate => DateTime.TryParse(firstDueDate.ToString(), out var result))
                .WithMessage("A Data de primeiro vencimento se inserida deve ser uma data válida")
                .When(client => client.FirstDueDate != null);

            RuleFor(product => product.QuantityPeriod)
                .Must(quantityPeriod => int.TryParse(quantityPeriod.ToString(), out var result) &&
                 result > 0)
                .WithMessage("A quantidade de períodos deve ser inserida");

            RuleFor(product => product.TimePeriod)
                .Must(timePeriod => ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.Contains(timePeriod.ToLower()))
                .WithMessage("O tipo de período deve ser informado corretamente")
                .MaximumLength(255)
                .WithMessage("O tipo de período ultrapassou o limite máximo de caracteres");
        }
    }
}