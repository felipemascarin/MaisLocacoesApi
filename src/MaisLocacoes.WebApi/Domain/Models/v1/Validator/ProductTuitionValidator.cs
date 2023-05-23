using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class ProductTuitionValidator : AbstractValidator<ProductTuitionRequest>
    {
        public ProductTuitionValidator()
        {
            RuleFor(productTuition => productTuition.RentId)
               .Must(rentId => int.TryParse(rentId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id da locação deve ser um número inteiro maior que 0");

            RuleFor(product => product.ProductType)
                .Must(productType => !string.IsNullOrEmpty(productType))
                .WithMessage("O Tipo de produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo de produto ultrapassou o limite máximo de caracteres");

            RuleFor(product => product.ProductCode)
                .Must(productCode => !string.IsNullOrEmpty(productCode))
                .WithMessage("O Código do produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Código do produto ultrapassou o limite máximo de caracteres");

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

            RuleFor(product => product.Parts)
                .Must(parts => int.TryParse(parts.ToString(), out var result) &&
                 result > 0)
                .WithMessage("A Quantidade de partes do produto deve ser inserida");
        }
    }
}