using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema.Authentication
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(login => login.Token)
                .Must(token => !string.IsNullOrEmpty(token))
                .WithMessage("O Jwt Token é obrigatório")
                .MaximumLength(3000)
                .WithMessage("O Jwt Token atingiu o limite máximo de 3000 caracteres");
        }
    }
}