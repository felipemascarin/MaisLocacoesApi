using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class ProductTuitionValueValidator : AbstractValidator<ProductTuitionValueRequest>
    {
        public ProductTuitionValueValidator()
        {

            RuleFor(productTuitionValue => productTuitionValue.ProductTypeId)
               .Must(productTypeId => int.TryParse(productTypeId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do tipo de produto deve ser um número inteiro maior que 0");

            RuleFor(productTuitionValue => productTuitionValue.QuantityPeriod)
               .Must(quantityPeriod => int.TryParse(quantityPeriod.ToString(), out var result) &&
                result > 0)
               .WithMessage("A quantidade de períodos deve ser inserida");

            RuleFor(productTuitionValue => productTuitionValue.TimePeriod)
                .Must(timePeriod => ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.Contains(timePeriod.ToLower()))
                .WithMessage("O tipo de período deve ser informado corretamente")
                .MaximumLength(255)
                .WithMessage("O tipo de período ultrapassou o limite máximo de caracteres");

            RuleFor(productTuitionValue => productTuitionValue.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("O Valor de locação do produto inserido não é válido");
        }
    }
}