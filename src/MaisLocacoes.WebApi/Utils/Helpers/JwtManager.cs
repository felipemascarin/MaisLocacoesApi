using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication;
using Microsoft.IdentityModel.Tokens;
using Repository.v1.IRepository.UserSchema;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MaisLocacoes.WebApi.Utils.Helpers
{
    public class User
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Cnpj { get; set; }
        public string Module { get; set; }
        public string TimeZone { get; set; }
    }

    public class JwtManager
    {
        public const string secret = "%ATpGhSb9Xy3@6uWEfZmmz%bbg^XxVe4hDa3!J$4^jLsCBBrpbR!LoU9uLb^XpUQc2yEaDWRK9#JRQ@wu@QbcsFnvb!%b8To#sQE%ivbpvYAEoR8p$iFK@nNDur5@MSzbY&86A$UrUy$GDhdwz^AW6Cz^aSbz2YhWFCwVF8Nd2D8LssKn#pSfbY7oD9HzGa&AQJsnEdgx!Z4wJ3UVf2i@RVDt2c@6Y8xWHg%MY2sns8wELSXsHvitNXMxowtG@kx6obzruFu%eeNcTRkpsd6^HM%UkC9BbB522X4LXinh6nn4HqMY&HDtwAK!^6YHBPHjhqA47m8erDseP23FGJ#MCZi%6xCVEjd72mF@W#xMC7bnbjW%SDAp4Rs3pXRJz&#@oVj7FrhZdhXiytUbUyRUS^hRS^MGZ75@@dhK6y7gcNYW2Sfj@JM$o7qxmwTp!nNdwCgd43zB9$CU&%NM#pd9N%e2wAH3kzAWAT8^xJ2QqzSWoQp3WGeZt5dk!#FMg&b";
        private readonly IUserRepository _userRepository;
        public static string Token { get; set; }

        public JwtManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public static LoginResponse CreateToken(User user, double tokenTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("cnpj", user.Cnpj),
                    new Claim("module", user.Module),
                    new Claim("cpf", user.Cpf),
                    new Claim("timeZone", user.TimeZone)
                }),
                Expires = System.DateTime.UtcNow.AddHours(tokenTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse
            {
                Token = tokenHandler.WriteToken(token)
            };
        }

        public static string ExtractTokenByAuthorization(IHttpContextAccessor httpContextAccessor)
        {
            if (!httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new HttpRequestException("Código acessado sem token válido", null, HttpStatusCode.BadRequest);
            }
            return httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
        }

        public static string GetTimeZoneByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "timeZone");
        }

        public static string GetEmailByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "email");
        }

        public static string GetCnpjByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "cnpj");
        }

        public static string GetModuleByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "module");
        }

        public static string GetCpfByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "cpf");
        }

        public static string GetRoleByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "role");
        }

        public static string GetDataBaseByToken(IHttpContextAccessor httpContextAccessor)
        {
            Token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(Token, "database");
        }

        public static string ExtractPropertyByToken(string token, string property)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var payload = decodedToken.Payload;
            return payload[property].ToString();
        }

        //Verifica se é o ultimo token criado para o usuário acessando
        public async Task<bool> UserHasToken(string token, string email, string cnpj)
        {
            if (await _userRepository.UserHasToken(token, email, cnpj))
                return true;
            else
                return false;
        }
    }
}