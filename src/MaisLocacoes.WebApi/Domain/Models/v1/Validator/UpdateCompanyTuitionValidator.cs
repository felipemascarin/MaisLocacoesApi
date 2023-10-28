using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateCompanyTuitionValidator : AbstractValidator<UpdateCompanyTuitionRequest>
    {
        public UpdateCompanyTuitionValidator()
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
        }
    }
}
