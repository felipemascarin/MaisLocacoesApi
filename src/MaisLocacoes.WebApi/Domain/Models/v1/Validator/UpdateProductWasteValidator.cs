using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateProductWasteValidator : AbstractValidator<UpdateProductWasteRequest>
    {
        public UpdateProductWasteValidator()
        {
            RuleFor(productWaste => productWaste.ProductId)
               .Must(productId => int.TryParse(productId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do produto deve ser um número inteiro maior que 0");

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
                .NotEmpty()
                .WithMessage("Data é obrigatória");
        }
    }
}