namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.User
{
    public class GetAllUsersByCnpjResponse
    {
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Rg { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime? BornDate { get; set; }
        public string Cel { get; set; }
        public string CivilStatus { get; set; }
        public string CpfDocumentUrl { get; set; }
        public string Status { get; set; }
        public List<string> Cnpjs { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
