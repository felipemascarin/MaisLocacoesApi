using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enums;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateBillValidator : AbstractValidator<UpdateBillRequest>
    {
        public UpdateBillValidator()
        {
            RuleFor(bill => bill.RentId)
                .Must(rentId => int.TryParse(rentId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id da locação deve ser um número inteiro maior que 0");

            RuleFor(bill => bill.ProductTuitionId)
                .Must(productTuitionId => int.TryParse(productTuitionId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id do ProductTuition se inserido deve ser um número inteiro maior que 0")
                .When(bill => bill.ProductTuitionId != null);

            RuleFor(bill => bill.Value)
                .Must(value => decimal.TryParse(value.ToString(), out var result) &&
                 result >= 0)
                .WithMessage("Valor da fatura inválido");

            RuleFor(bill => bill.NfIdFireBase)
                .Must(nfIdFireBase => int.TryParse(nfIdFireBase.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id da nota fiscal se inserido deve ser número")
                .When(bill => bill.NfIdFireBase != null);

            RuleFor(bill => bill.Order)
                .Must(nfIdFireBase => int.TryParse(nfIdFireBase.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Número para ordenação da nota fiscal se inserido deve ser número inteiro maior que 0")
                .When(bill => bill.Order != null);

            RuleFor(bill => bill.PaymentMode)
                .Must(paymentMode => PaymentModes.PaymentModesEnum.Contains(paymentMode))
                .WithMessage("Modo de pagamento não existente")
                .MaximumLength(255)
                .WithMessage("Modo de pagamento ultrapassou o limite máximo de caracteres")
                .When(bill => bill.PaymentMode != null);

            RuleFor(bill => bill.Description)
                .MaximumLength(1000)
                .WithMessage("Descrição ultrapassou o limite máximo de caracteres")
                .When(bill => !string.IsNullOrEmpty(bill.Description));

            RuleFor(bill => bill.DueDate)
                .NotEmpty()
                .WithMessage("Data de vencimento é obrigatória");
        }
    }
}
