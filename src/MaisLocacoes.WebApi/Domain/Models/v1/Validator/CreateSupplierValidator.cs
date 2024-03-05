using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Helpers;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierValidator()
        {
            RuleFor(supplier => supplier.Name)
                .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage("O Nome do fornecedor deve ser informado")
                .MaximumLength(255)
                .WithMessage("O Nome do fornecedor ultrapassou o limite máximo de caracteres");

            RuleFor(supplier => supplier.Cnpj)
                .Must(cnpj => DocumentValidator.IsCnpj(cnpj))
                .WithMessage("O CNPJ se informado deve ser válido")
                .When(supplier => !string.IsNullOrEmpty(supplier.Cnpj));

            RuleFor(supplier => supplier.Cnpj)
                .Matches(@"^\d{14}$")
                .WithMessage("O CNPJ se inserido deve ser somente os números")
                .When(supplier => !string.IsNullOrEmpty(supplier.Cnpj));

            RuleFor(supplier => supplier.Cel)
                .Must(cel => long.TryParse(cel, out var result) && result > 0)
                .WithMessage("Celular se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Celular ultrapassou o limite máximo de caracteres")
                .When(supplier => !string.IsNullOrEmpty(supplier.Cel));

            RuleFor(supplier => supplier.Tel)
                .Must(tel => long.TryParse(tel, out var result) && result > 0)
                .WithMessage("Telefone se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Telefone ultrapassou o limite máximo de caracteres")
                .When(supplier => !string.IsNullOrEmpty(supplier.Tel));

            RuleFor(supplier => supplier.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email se informado deve ter formato de um e-mail")
                .MaximumLength(255)
                .WithMessage("Email ultrapassou o limite máximo de caracteres")
                .When(supplier => !string.IsNullOrEmpty(supplier.Email));

            RuleFor(supplier => supplier.Address).SetValidator(new CreateAddressValidator());
        }
    }
}