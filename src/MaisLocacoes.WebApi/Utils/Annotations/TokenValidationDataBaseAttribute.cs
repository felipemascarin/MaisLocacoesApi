using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.v1.IRepository.UserSchema;

namespace MaisLocacoes.WebApi.Utils.Annotations
{
    public class TokenValidationDataBaseAttribute : TypeFilterAttribute
    {
        public TokenValidationDataBaseAttribute() : base(typeof(TokenValidationFilter)) { }

        private class TokenValidationFilter : IAsyncAuthorizationFilter
        {
            private readonly IUserRepository _userRepository;

            public TokenValidationFilter(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }
           
            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];

                var email = JwtManager.ExtractPropertyByToken(token, "email");

                var cnpj = JwtManager.ExtractPropertyByToken(token, "cnpj");

                var tokenExists = await _userRepository.UserHasToken(token, email);

                if (!tokenExists)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}