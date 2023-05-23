using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema
{
    public class GetCompanyResponse
    {
        public string Cnpj { get; set; }
        public int AddressId { get; set; }
        public string CompanyName { get; set; }
        public string? StateRegister { get; set; }
        public string FantasyName { get; set; }
        public string Cel { get; set; }
        public string? Tel { get; set; }
        public string Email { get; set; }
        public string Segment { get; set; }
        public string PjDocumentUrl { get; set; }
        public string AddressDocumentUrl { get; set; }
        public string? LogoUrl { get; set; }
        public int NotifyDaysBefore { get; set; }
        public string Status { get; set; }
        public GetCompanyAddressResponse CompanyAddress { get; set; }
    }
}