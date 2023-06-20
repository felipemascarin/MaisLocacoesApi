using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class BillValidator : AbstractValidator<BillRequest>
    {
        public BillValidator()
        {
            RuleFor(bill => bill.ProductTuitionId)
                .Must(productTuitionId => int.TryParse(productTuitionId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id do ProductTuition deve ser um número inteiro maior que 0");

            RuleFor(bill => bill.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor da fatura inválido");

            RuleFor(bill => bill.PayDate)
                .Must(payDate => DateTime.TryParse(payDate.ToString(), out var result))
                .WithMessage("A data de pagamento se inserida deve ser uma data válida")
                .When(bill => bill.PayDate != null);

            RuleFor(bill => bill.DueDate)
                .Must(dueDate => DateTime.TryParse(dueDate.ToString(), out var result))
                .WithMessage("A Data de vencimento é obrigatória");

            RuleFor(bill => bill.NfIdFireBase)
                .Must(nfIdFireBase => int.TryParse(nfIdFireBase.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id da nota fiscal se inserido deve ser número")
                .When(bill => bill.NfIdFireBase != null);

            RuleFor(bill => bill.PaymentMode)
                .MaximumLength(255)
                .WithMessage("Modo de pagamento ultrapassou o limite máximo de caracteres")
                .When(bill => !string.IsNullOrEmpty(bill.PaymentMode));
        }
    }
}