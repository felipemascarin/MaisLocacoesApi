using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CreateContractValidator : AbstractValidator<CreateContractRequest>
    {
        public CreateContractValidator()
        {
            RuleFor(contract => contract.RentId)
               .Must(rentId => int.TryParse(rentId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id da locação deve ser um número inteiro maior que 0");

            RuleFor(contract => contract.UrlSignature)
                .MaximumLength(2048)
                .WithMessage("UrlSignature ultrapassou o limite máximo de caracteres")
                .When(contract => !string.IsNullOrEmpty(contract.UrlSignature));
        }
    }
}
