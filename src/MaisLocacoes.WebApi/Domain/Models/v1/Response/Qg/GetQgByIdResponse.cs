namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Qg
{
    public class GetQgByIdResponse
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AddressResponse Address { get; set; }

        public class AddressResponse
        {
            public int Id { get; set; }
            public string Cep { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string Complement { get; set; }
            public string District { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
}
