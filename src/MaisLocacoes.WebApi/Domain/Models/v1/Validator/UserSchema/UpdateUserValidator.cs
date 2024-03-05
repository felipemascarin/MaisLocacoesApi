using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(person => person.Cpf)
                .Must(cpf => !string.IsNullOrEmpty(cpf))
                .WithMessage("O CPF é obrigatório");

            RuleFor(person => person.Cpf)
                .Must(cpf => DocumentValidator.IsCpf(cpf))
                .WithMessage("O CPF informado é inválido")
                .When(person => !string.IsNullOrEmpty(person.Cpf));

            RuleFor(person => person.Cpf)
                .Matches(@"^\d{11}$")
                .WithMessage("O CPF se inserido deve ser informado somente os números")
                .When(person => !string.IsNullOrEmpty(person.Cpf));

            RuleFor(person => person.Rg)
               .Must(rg => long.TryParse(rg, out var result) && result > 0)
               .WithMessage("RG se informado deve conter somente números")
               .MaximumLength(18)
               .WithMessage("RG ultrapassou o limite máximo de caracteres")
               .When(person => !string.IsNullOrEmpty(person.Rg));

            RuleFor(person => person.Name)
                .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage("O Nome é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Nome ultrapassou o limite máximo de caracteres");

            RuleFor(person => person.Email)
                .Must(email => !string.IsNullOrEmpty(email))
                .WithMessage("Email é obrigatório")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email deve ter formato de um e-mail")
                .MaximumLength(255)
                .WithMessage("Email ultrapassou o limite máximo de caracteres");

            RuleFor(person => person.Role)
                .Must(role => !string.IsNullOrEmpty(role))
                .WithMessage("A Role de autorização é obrigatório")
                .MaximumLength(255)
                .WithMessage("A Role de autorização ultrapassou o limite máximo de caracteres");

            RuleFor(person => person.Role)
                .Must(role => UserRole.PersonRolesEnum.Contains(role.ToLower()))
                .WithMessage("A Role inserida não existe")
                .When(person => !string.IsNullOrEmpty(person.Role));

            RuleFor(person => person.BornDate)
                .Must(bornDate => bornDate < DateTime.Now.Date)
                .WithMessage("A data de nascimento se inserida deve ser data passada")
                .When(person => person.BornDate != null);

            RuleFor(person => person.Cel)
                .Must(tel => long.TryParse(tel, out var result) && result > 0)
                .WithMessage("Celular se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Celular ultrapassou o limite máximo de caracteres")
                .When(person => !string.IsNullOrEmpty(person.Cel));

            RuleFor(person => person.CivilStatus)
                .MaximumLength(255)
                .WithMessage("Estado civil ultrapassou o limite máximo de caracteres")
                .When(person => !string.IsNullOrEmpty(person.CivilStatus));

            RuleFor(person => person.ProfileImageUrl)
               .MaximumLength(2048)
               .WithMessage("A URL da imagem de perfil ultrapassou o limite máximo de caracteres")
               .When(person => !string.IsNullOrEmpty(person.ProfileImageUrl));

            RuleFor(person => person.CpfDocumentUrl)
               .MaximumLength(2048)
               .WithMessage("A URL do documento ultrapassou o limite máximo de caracteres")
               .When(person => !string.IsNullOrEmpty(person.CpfDocumentUrl));
        }
    }
}
