using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(product => product.Code)
                .Must(code => !string.IsNullOrEmpty(code))
                .WithMessage("O Código do produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Código do produto ultrapassou o limite máximo de caracteres");

            RuleFor(product => product.ProductTypeId)
               .Must(productTypeId => int.TryParse(productTypeId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do tipo de produto deve ser um número inteiro maior que 0");

            RuleFor(product => product.SupplierId)
                .Must(supplierId => int.TryParse(supplierId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("O Id do Fornecedor se inserido deve ser um número positivo")
                .When(product => product.SupplierId != null);

            RuleFor(product => product.Description)
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres")
                .When(product => !string.IsNullOrEmpty(product.Description));

            RuleFor(product => product.DateBought)
                .Must(dateBought => DateTime.TryParse(dateBought.ToString(), out var result))
                .WithMessage("A Data de compra se inserida deve ser uma data válida")
                .When(product => product.DateBought != null);

            RuleFor(product => product.BoughtValue)
                .Must(boughtValue => decimal.TryParse(boughtValue.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("O Valor de compra se inserido deve ser um número")
                .When(product => product.BoughtValue != null);

            RuleFor(product => product.CurrentRentedPlaceId)
                .Must(currentRentedPlaceId => int.TryParse(currentRentedPlaceId.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("O Id da localidade atual do produto se inserido deve ser um número positivo")
                .When(product => product.CurrentRentedPlaceId != null);

                RuleFor(product => product.Parts)
                .Must(parts => int.TryParse(parts.ToString(), out var result) &&
                 result > 0)
                .WithMessage("A quantidade de peças do produto deve ser um número inteiro maior que 0")
                .When(product => product.CurrentRentedPlaceId != null);

            RuleFor(product => product.Parts)
                .Must(parts => int.TryParse(parts.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("A quantidade de peças alugadas do produto se inserido deve ser um número positivo")
                .When(product => product.CurrentRentedPlaceId != null);
        }
    }
}