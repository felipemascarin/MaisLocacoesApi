namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public IEnumerable<CompanyUserDto> CompanyUser { get; set; }

        public class CompanyUserDto
        {
            public string CompanyName { get; set; }
            public string Cnpj { get; set; }
        }
    }
}