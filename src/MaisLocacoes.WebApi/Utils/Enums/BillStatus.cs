namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class BillStatus
    {
        public static IEnumerable<string> BillStatusEnum = new List<string>
        {
            "open",
            "payed",
            "dued",
            "canceled"
        };
    }
}