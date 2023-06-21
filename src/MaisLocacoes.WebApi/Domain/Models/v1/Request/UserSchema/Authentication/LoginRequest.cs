namespace MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication
{
    public class LoginRequest
    {
        public string GoogleToken { get; set; }
        public string Cnpj { get; set; }
    }
}