using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class ProductWasteValidator : AbstractValidator<ProductWasteRequest>
    {
        public ProductWasteValidator()
        {
            RuleFor(productWaste => productWaste.ProductType)
                .Must(productType => !string.IsNullOrEmpty(productType))
                .WithMessage("O Tipo de produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo de produto ultrapassou o limite máximo de caracteres");

            RuleFor(productWaste => productWaste.ProductCode)
                .Must(productCode => !string.IsNullOrEmpty(productCode))
                .WithMessage("O Código do produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Código do produto ultrapassou o limite máximo de caracteres");

            RuleFor(productWaste => productWaste.Description)
                .Must(description => !string.IsNullOrEmpty(description))
                .WithMessage("A Descrição é obrigatória")
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres");

            RuleFor(productWaste => productWaste.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor do gasto inválido");

            RuleFor(productWaste => productWaste.Date)
                .Must(dueDate => DateTime.TryParse(dueDate.ToString(), out var result))
                .WithMessage("A Data do gasto é obrigatória");
        }
    }
}