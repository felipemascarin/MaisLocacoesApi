namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class RentStatus
    {
        public static IEnumerable<string> RentStatusEnum = new List<string>
        {
            "activated",
            "finished",
            "canceled"
        };
    }
}