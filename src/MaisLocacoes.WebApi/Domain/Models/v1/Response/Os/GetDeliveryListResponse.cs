namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Os
{
    public class GetDeliveryListResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ClientResponse Client { get; set; }
        public RentAddressResponse Address { get; set; }
        public List<ProductTuitionResponse> ProductTuitions { get; set; }

        public class ClientResponse
        {
            public string FantasyName { get; set; }
            public string ClientName { get; set; }
            public string Cel { get; set; }
        }

        public class RentAddressResponse
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
        }

        public class ProductTuitionResponse
        {
            public int Id { get; set; }
            public string ProductCode { get; set; }
            public decimal Value { get; set; }
            public DateTime InitialDateTime { get; set; }
            public int Parts { get; set; }
            public List<OsResponse> Oss { get; set; }
            public ProductTypeResponse ProductType { get; set; }
        }

        public class OsResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string DeliveryCpf { get; set; }
            public string Status { get; set; }
            public DateTime? InitialDateTime { get; set; }
            public DateTime? FinalDateTime { get; set; }
            public string DeliveryObservation { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class ProductTypeResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public bool IsManyParts { get; set; }
        }

    }
}
