using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator.UserSchema.Authentication
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(login => login.GoogleToken)
               .Must(googleToken => !string.IsNullOrEmpty(googleToken))
               .WithMessage("O Jwt Token é obrigatório")
               .MaximumLength(3000)
               .WithMessage("O Jwt Token atingiu o limite máximo de 3000 caracteres");
        }
    }
}