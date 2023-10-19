using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateCompanyWasteValidator : AbstractValidator<UpdateCompanyWasteRequest>
    {
        public UpdateCompanyWasteValidator()
        {
            RuleFor(companyWaste => companyWaste.Description)
                .Must(description => !string.IsNullOrEmpty(description))
                .WithMessage("A Descrição é obrigatória")
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres");

            RuleFor(companyWaste => companyWaste.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor do gasto inválido");

            RuleFor(companyWaste => companyWaste.Date)
                .Must(dueDate => DateTime.TryParse(dueDate.ToString(), out var result))
                .WithMessage("A Data do gasto é obrigatória");
        }
    }
}
