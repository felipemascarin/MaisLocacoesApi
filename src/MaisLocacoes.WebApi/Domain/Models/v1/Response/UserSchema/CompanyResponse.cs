namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema
{
    public class CompanyResponse
    {
        public string Cnpj { get; set; }
        public int AddressId { get; set; }
        public string CompanyName { get; set; }
        public string StateRegister { get; set; }
        public string FantasyName { get; set; }
        public string Cel { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Segment { get; set; }
        public string PjDocumentUrl { get; set; }
        public string AddressDocumentUrl { get; set; }
        public string LogoUrl { get; set; }
        public int NotifyDaysBefore { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CompanyAddressResponse CompanyAddress { get; set; }
    }
}