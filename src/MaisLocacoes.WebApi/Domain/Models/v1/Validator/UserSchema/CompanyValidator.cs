using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema
{
    public class CompanyValidator : AbstractValidator<CompanyRequest>
    {
        public CompanyValidator()
        {
            RuleFor(company => company.Cnpj)
                .Must(cnpj => !string.IsNullOrEmpty(cnpj))
                .WithMessage("O CNPJ é obrigatório");

            RuleFor(company => company.Cnpj)
                .Must(cnpj => DocumentValidator.IsCnpj(cnpj))
                .WithMessage("O CNPJ informado é inválido")
                .When(company => !string.IsNullOrEmpty(company.Cnpj));

            RuleFor(company => company.CompanyName)
                .Must(companyName => !string.IsNullOrEmpty(companyName))
                .WithMessage("Razão social é obrigatória")
                .MaximumLength(255)
                .WithMessage("Razão social ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.FantasyName)
                .Must(fantasyName => !string.IsNullOrEmpty(fantasyName))
                .WithMessage("O Nome Fantasia é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Nome Fantasia ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.StateRegister)
                .Must(stateRegister => long.TryParse(stateRegister, out var result) && result > 0)
                .WithMessage("Inscrição Estadual se informada deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Inscrição Estadual ultrapassou o limite máximo de caracteres")
                .When(company => !string.IsNullOrEmpty(company.StateRegister));

            RuleFor(company => company.Cel)
                .Must(cel => long.TryParse(cel, out var result) && result > 0)
                .WithMessage("Celular é obrigatório e deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Celular ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.Tel)
                .Must(tel => long.TryParse(tel, out var result) && result > 0)
                .WithMessage("Telefone se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Telefone ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Tel));

            RuleFor(company => company.Email)
                .Must(email => !string.IsNullOrEmpty(email))
                .WithMessage("Email é obrigatório")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email deve ter formato de um e-mail")
                .MaximumLength(255)
                .WithMessage("Email ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.Segment)
                .Must(segment => !string.IsNullOrEmpty(segment))
                .WithMessage("O Ramo da empresa é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Ramo da empresa ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.PjDocumentUrl)
                .Must(pjDocumentUrl => !string.IsNullOrEmpty(pjDocumentUrl))
                .WithMessage("A URL do documento da empresa é obrigatória")
                .MaximumLength(2048)
                .WithMessage("A URL do documento da empresa ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.AddressDocumentUrl)
                .Must(addressDocumentUrl => !string.IsNullOrEmpty(addressDocumentUrl))
                .WithMessage("A URL do comprovante de endereço da empresa é obrigatória")
                .MaximumLength(2048)
                .WithMessage("A URL do comprovante de endereço da empresa ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.LogoUrl)
                .MaximumLength(2048)
                .WithMessage("A URL da logo da empresa ultrapassou o limite máximo de caracteres");

            RuleFor(company => company.NotifyDaysBefore)
                .Must(notifyDaysBefore => int.TryParse(notifyDaysBefore.ToString(), out var result) &&
                 result >= 0 && result <= 500)
                .WithMessage("Notificar dias antes deve ser um número de 0 à 500");

            RuleFor(company => company.CompanyAddress).SetValidator(new CompanyAddressValidator());
        }
    }
}