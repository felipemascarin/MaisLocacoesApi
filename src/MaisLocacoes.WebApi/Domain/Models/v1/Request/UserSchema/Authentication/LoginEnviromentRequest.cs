namespace MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication
{
    public class LoginEnviromentRequest
    {
        public string Token { get; set; }
        public string Cnpj { get; set; }
    }
}
