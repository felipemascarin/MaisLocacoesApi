using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Utils.Helpers;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema.Authentication
{
    public class LoginEnviromentRequestValidator : AbstractValidator<LoginEnviromentRequest>
    {
        public LoginEnviromentRequestValidator()
        {
            RuleFor(modifyLoginToken => modifyLoginToken.Token)
               .Must(token => !string.IsNullOrEmpty(token))
               .WithMessage("O Jwt Token é obrigatório")
               .MaximumLength(3000)
               .WithMessage("O Jwt Token atingiu o limite máximo de 3000 caracteres");

            RuleFor(person => person.Cnpj)
                .Must(cnpj => !string.IsNullOrEmpty(cnpj))
                .WithMessage("O CNPJ é obrigatório");

            RuleFor(company => company.Cnpj)
                .Must(cnpj => DocumentValidator.IsCnpj(cnpj))
                .WithMessage("O CNPJ informado é inválido");
        }
    }
}
