using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CompanyTuitionValidator : AbstractValidator<CompanyTuitionRequest>
    {
        public CompanyTuitionValidator()
        {
            RuleFor(companyTuition => companyTuition.AsaasNumber)
                .Must(asaasNumber => int.TryParse(asaasNumber.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Número da fatura do Asaas inválido")
                .When(companyTuition => companyTuition.AsaasNumber != null);

            RuleFor(companyTuition => companyTuition.TuitionNumber)
                .Must(tuitionNumber => int.TryParse(tuitionNumber.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Número da fatura do Asaas inválido")
                .When(companyTuition => companyTuition.TuitionNumber != null);

            RuleFor(companyTuition => companyTuition.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor da fatura inválido");

            RuleFor(companyTuition => companyTuition.PayDate)
                .Must(payDate => DateTime.TryParse(payDate.ToString(), out var result))
                .WithMessage("A data de pagamento se inserida deve ser uma data válida")
                .When(companyTuition => companyTuition.PayDate != null);

            RuleFor(companyTuition => companyTuition.DueDate)
                .Must(dueDate => DateTime.TryParse(dueDate.ToString(), out var result))
                .WithMessage("A Data de vencimento é obrigatória");
        }
    }
}