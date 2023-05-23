using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class ProductTypeValidator : AbstractValidator<ProductTypeRequest>
    {
        public ProductTypeValidator()
        {
            RuleFor(productType => productType.Type)
                .Must(type => !string.IsNullOrEmpty(type))
                .WithMessage("O Tipo do produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo do produto ultrapassou o limite máximo de caracteres");
        }
    }
}